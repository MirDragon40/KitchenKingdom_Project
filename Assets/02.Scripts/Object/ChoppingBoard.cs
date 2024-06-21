using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChoppingBoard : CookStand
{
    public Slider ChoppingProgressSlider;
    public Animator Animator;
    private Coroutine fillSliderCoroutine;
    private bool _isPossibleChopping = false;
    public OnChoppingBoard_Collider onChoppingBoard;
    private PhotonView _photonView;
    public SoundManager SoundManager;
    

    

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        ChoppingProgressSlider.value = 0f; // 슬라이더 초기화
        ChoppingProgressSlider.gameObject.SetActive(false);
        SoundManager = FindObjectOfType<SoundManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (_isPossibleChopping && onChoppingBoard.FoodOnBoard != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && PlacedItem != null)
            {
                if (fillSliderCoroutine != null)
                {
                    StopCoroutine(fillSliderCoroutine);
                }
                _photonView.RPC("StartChoppingRPC", RpcTarget.All, 3.0f);
                SoundManager.PlayAudio("Cut", false, true);
            }

            
        }
    }

    [PunRPC]
    private void StartChoppingRPC(float duration)
    {
        if (fillSliderCoroutine != null)
        {
            StopCoroutine(fillSliderCoroutine);
        }
        onChoppingBoard.FoodObject.StartCooking();
        ChoppingProgressSlider.gameObject.SetActive(true);
        fillSliderCoroutine = StartCoroutine(FillSliderOverTime(duration));
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && onChoppingBoard.FoodOnBoard != null)
        {
            _isPossibleChopping = true;
        }

        if (other.CompareTag("Player") && !IsOccupied)
        {
            CharacterHoldAbility characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
            if (characterHoldAbility != null)
            {
                characterHoldAbility.IsPlaceable = true;
                characterHoldAbility.PlacePosition = PlacePosition;
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _isPossibleChopping)
        {
            _isPossibleChopping = false;

            if (fillSliderCoroutine != null)
            {
                StopCoroutine(fillSliderCoroutine);
            }

            if (onChoppingBoard.FoodObject != null)
            {
                onChoppingBoard.FoodObject.StopCooking();
                SoundManager.StopAudio("Cut");
            }

            Animator.SetBool("Chopping", false); // 슬라이더가 진행되지 않을 때 애니메이션을 중지
            _photonView.RPC("StopChoppingRPC", RpcTarget.All);
        }

        if (other.CompareTag("Player"))
        {
            CharacterHoldAbility characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
            if (characterHoldAbility != null)
            {
                characterHoldAbility.IsPlaceable = false;
                characterHoldAbility.PlacePosition = null;
            }
        }
    }


    [PunRPC]
    private void StopChoppingRPC()
    {
        Animator.SetBool("Chopping", false); // 슬라이더가 완료되면 애니메이션을 중지
        ChoppingProgressSlider.gameObject.SetActive(false);
    }

    private IEnumerator FillSliderOverTime(float duration)
    {
        FoodObject foodObject = onChoppingBoard.FoodObject;

        if (foodObject == null)
            yield break;

        Animator.SetBool("Chopping", true); // 슬라이더가 진행될 때 애니메이션을 시작

        float startProgress = foodObject.CookProgress;
        float elapsedTime = startProgress * duration;

        while (elapsedTime < duration)
        {
            if (!foodObject.IsCooking)
            {
                yield return null;
                continue;
            }

            elapsedTime += Time.deltaTime;
            foodObject.CookProgress = Mathf.Clamp01(elapsedTime / duration);
            ChoppingProgressSlider.value = foodObject.CookProgress;

            if (foodObject.CookProgress >= 1f)
            {
                break;
            }

            yield return null;
        }

        Animator.SetBool("Chopping", false); // 슬라이더가 완료되면 애니메이션을 중지
        foodObject.StopCooking();
        ChoppingProgressSlider.gameObject.SetActive(false);
        SoundManager.StopAudio("Cut");
    }
}
