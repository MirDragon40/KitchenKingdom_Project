using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    private void Awake()
    {
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
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.LeftControl))
        {
            ProgressSlider.gameObject.SetActive(true);

            if (washingCoroutine == null)
            {
                washingCoroutine = StartCoroutine(WashPlates());
            }
        }

        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space) && CleanPlateNum > 0)
        {

            TakeCleanPlate();
        }

        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space) && dirtyPlate != null)
        {
            Debug.Log(DirtyPlateNum);
            GetDirtyPlateNum();
            dishObject.Place(PlacePosition);
            characterHoldAbility.Place();
            Destroy(dirtyPlate.gameObject);
        }
    }

    private IEnumerator WashPlates()
    {
        while (DirtyPlateNum > 0)
        {
            // 슬라이더 값을 4초 동안 증가
            float duration = 4f;
            float elapsed = ProgressSlider.value * duration; // 이전 진행도에서 시작

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                ProgressSlider.value = Mathf.Clamp01(elapsed / duration);

                BubbleEffect.SetActive(true);

                yield return null;

                if (!isPlayerInTrigger)
                {
                    // 플레이어가 트리거를 벗어나면 진행도를 유지하고 코루틴 일시정지
                    washingCoroutine = null;

                    BubbleEffect.SetActive(false);

                    yield break;
                }
            }

            // PlateNum 감소
            DirtyPlateNum--;
            CleanPlateNum++;
            UpdatePlates();

            // 진행도 리셋
            ProgressSlider.value = 0f;
        }

        // Progress가 1 이 되면 멈춰있도록
        ProgressSlider.value = 1f;
        washingCoroutine = null;
        BubbleEffect.SetActive(false);
        ProgressSlider.gameObject.SetActive(false);
    }

    private void UpdatePlates()
    {
        // DirtyPlates 업데이트
        for (int i = 0; i < DirtyPlates.Count; i++)
        {
            if (i < DirtyPlateNum)
            {
                DirtyPlates[i].SetActive(true);
            }
            else
            {
                DirtyPlates[i].SetActive(false);
            }
        }

        // CleanPlates 업데이트
        for (int i = 0; i < CleanPlates.Count; i++)
        {
            if (i < CleanPlateNum)
            {
                CleanPlates[i].SetActive(true);
            }
            else
            {
                CleanPlates[i].SetActive(false);
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
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
            dirtyPlate = null;
            dishObject = null;
        }
    }

    private void TakeCleanPlate()
    {
        CleanPlateNum--;
        UpdatePlates();
        characterHoldAbility.SpawnPlateOnHand();
    }

    private void GetDirtyPlateNum()
    {

        DirtyPlateNum = DirtyPlateNum + dirtyPlate.DirtyPlateNum;
        UpdatePlates();
        dirtyPlate.DirtyPlateNum = 0;

    }
}
