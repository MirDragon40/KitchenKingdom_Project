using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sink : MonoBehaviourPun
{
    public Transform PlacePosition;

    public int DirtyPlateNum;
    public int CleanPlateNum;

    public Slider ProgressSlider;
    private Coroutine washingCoroutine;
    private bool isPlayerInTrigger = false;

    public List<GameObject> DirtyPlates;
    public List<GameObject> CleanPlates;

    public GameObject BubbleEffect;

    private CharacterHoldAbility characterHoldAbility;
    private DirtyPlate dirtyPlate;
    private DishObject dishObject;

    private PhotonView _pv;

    private Character _nearbyCharacter;
    private bool isPlayerNearby => _nearbyCharacter != null;

    private bool isWashing = false;

    private void Awake()
    {
        _pv = GetComponent<PhotonView>();

        BubbleEffect.SetActive(false);

        foreach (GameObject plate in DirtyPlates)
        {
            plate.SetActive(false);
        }

        foreach (GameObject plate in CleanPlates)
        {
            plate.SetActive(false);
        }

        ProgressSlider.gameObject.SetActive(false);
    }

    private void Start()
    {
        ProgressSlider.value = 0f;
        UpdatePlates();
    }

    private void Update()
    {
        if (!isPlayerNearby)
        {
            return;
        }

        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.LeftControl) && _nearbyCharacter.PhotonView.IsMine)
        {
            ProgressSlider.gameObject.SetActive(true);

            if (washingCoroutine == null)
            {
                washingCoroutine = StartCoroutine(WashPlates());
            }
        }

        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space) && CleanPlateNum > 0 && _nearbyCharacter.PhotonView.IsMine)
        {
            TakeCleanPlate();
        }

        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space) && dirtyPlate != null && _nearbyCharacter.PhotonView.IsMine)
        {
            PlaceDirtyPlateInSink();
        }
    }

    private void PlaceDirtyPlateInSink()
    {
        _pv.RPC(nameof(UpdateDirtyPlateNum), RpcTarget.AllBuffered, DirtyPlateNum + dirtyPlate.DirtyPlateNum);
        dishObject.Place(PlacePosition);
        characterHoldAbility.Place();
        _pv.RPC(nameof(RemoveDirtyPlate), RpcTarget.AllBuffered, dirtyPlate.PhotonView.ViewID);
    }

    [PunRPC]
    private void RemoveDirtyPlate(int dirtyPlateID)
    {
        PhotonView dirtyPlateView = PhotonView.Find(dirtyPlateID);
        if (dirtyPlateView != null)
        {
            DirtyPlate plate = dirtyPlateView.GetComponent<DirtyPlate>();
            if (plate != null)
            {
                plate.RemoveDirtyPlate();
            }
        }
    }

    private IEnumerator WashPlates()
    {
        _pv.RPC(nameof(UpdateWashingState), RpcTarget.AllBuffered, true);
        SoundManager.Instance.PlayAudio("DishWash", false, true);

        while (DirtyPlateNum > 0)
        {
            float duration = 4f;
            float elapsed = ProgressSlider.value * duration;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                ProgressSlider.value = Mathf.Clamp01(elapsed / duration);

                _pv.RPC(nameof(UpdateSliderAndEffect), RpcTarget.AllBuffered, ProgressSlider.value, true);

                yield return null;

                if (!isPlayerInTrigger)
                {
                    washingCoroutine = null;

                    _pv.RPC(nameof(UpdateSliderAndEffect), RpcTarget.AllBuffered, ProgressSlider.value, false);

                    yield break;
                }
            }

            DirtyPlateNum--;
            CleanPlateNum++;
            _pv.RPC(nameof(UpdatePlateNums), RpcTarget.AllBuffered, DirtyPlateNum, CleanPlateNum);

            ProgressSlider.value = 0f;
        }

        _pv.RPC(nameof(UpdateWashingState), RpcTarget.AllBuffered, false);
        SoundManager.Instance.StopAudio("DishWash");

        ProgressSlider.value = 1f;
        washingCoroutine = null;
        _pv.RPC(nameof(UpdateSliderAndEffect), RpcTarget.AllBuffered, 1f, false);
        ProgressSlider.gameObject.SetActive(false);
    }

    [PunRPC]
    private void UpdatePlateNums(int newDirtyPlateNum, int newCleanPlateNum)
    {
        DirtyPlateNum = newDirtyPlateNum;
        CleanPlateNum = newCleanPlateNum;
        UpdatePlates();
    }

    [PunRPC]
    private void UpdateSliderAndEffect(float sliderValue, bool isEffectActive)
    {
        ProgressSlider.value = sliderValue;
        BubbleEffect.SetActive(isEffectActive);
    }

    [PunRPC]
    private void UpdateWashingState(bool newIsWashing)
    {
        isWashing = newIsWashing;
        ProgressSlider.gameObject.SetActive(isWashing);
    }

    private void UpdatePlates()
    {
        for (int i = 0; i < DirtyPlates.Count; i++)
        {
            DirtyPlates[i].SetActive(i < DirtyPlateNum);
        }

        for (int i = 0; i < CleanPlates.Count; i++)
        {
            CleanPlates[i].SetActive(i < CleanPlateNum);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlayerNearby)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
            _nearbyCharacter = other.GetComponent<Character>();
            dirtyPlate = characterHoldAbility.gameObject.GetComponentInChildren<DirtyPlate>();
            dishObject = characterHoldAbility.gameObject.GetComponentInChildren<DishObject>();

            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            characterHoldAbility = null;
            _nearbyCharacter = null;

            dirtyPlate = null;
            dishObject = null;
            SoundManager.Instance.StopAudio("DishWash");
        }
    }

    private void TakeCleanPlate()
    {
        _pv.RPC(nameof(UpdatePlateNums), RpcTarget.AllBuffered, DirtyPlateNum, CleanPlateNum - 1);

        if (_nearbyCharacter != null)
        {
            _nearbyCharacter.PhotonView.RPC("RequestSpawnPlateOnHand", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void UpdateDirtyPlateNum(int newDirtyPlateNum)
    {
        DirtyPlateNum = newDirtyPlateNum;
        UpdatePlates();
    }
}
