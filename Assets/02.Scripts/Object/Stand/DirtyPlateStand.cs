using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlateStand : MonoBehaviour
{
    public List<GameObject> DirtyPlates;
    public int DirtyPlateNum = 5;

    private CharacterHoldAbility characterHoldAbility;
    private Character _nearbyCharacter;

    private DirtyPlate dirtyPlate;

    private bool isPlayerInTrigger = false;
    private bool isPlayerHoldingDirtyPlate = false;
    private bool isPlayerNearby => _nearbyCharacter != null;

    private void Awake()
    {
        foreach (GameObject plate in DirtyPlates)
        {
            plate.SetActive(false);
        }
    }

    private void Start()
    {
        UpdatePlates();
    }

    private void Update()
    {
        if (!isPlayerNearby)
        {
            return;
        }

        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space) && _nearbyCharacter.PhotonView.IsMine)
        {
            Debug.Log(DirtyPlateNum);
            GiveDirtyPlates();
        }
    }

    private void UpdatePlates()
    {
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
            isPlayerInTrigger = true;

            UpdateDirtyPlateStatus();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && characterHoldAbility != null)
        {
            UpdateDirtyPlateStatus();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            isPlayerHoldingDirtyPlate = false;

            dirtyPlate = null;
            characterHoldAbility = null;
            _nearbyCharacter = null;
        }
    }

    private void GiveDirtyPlates()
    {
        if (DirtyPlateNum > 0 && characterHoldAbility != null)
        {
            UpdateDirtyPlateStatus();

            if (isPlayerHoldingDirtyPlate)
            {
                // 플레이어가 이미 더러운 접시를 가지고 있는 경우
                dirtyPlate.DirtyPlateNum += DirtyPlateNum;
            }
            else
            {
                // 플레이어가 더러운 접시를 가지고 있지 않은 경우 새로운 접시 생성
                characterHoldAbility.SpawnDirtyPlateOnHand();

                // 다시 dirtyPlate를 확인하고 업데이트
                dirtyPlate = characterHoldAbility.gameObject.GetComponentInChildren<DirtyPlate>();
                if (dirtyPlate != null)
                {
                    dirtyPlate.DirtyPlateNum = DirtyPlateNum;
                }
            }
            DirtyPlateNum = 0;
            UpdatePlates();
        }
    }

    private void UpdateDirtyPlateStatus()
    {
        if (characterHoldAbility != null)
        {
            dirtyPlate = characterHoldAbility.gameObject.GetComponentInChildren<DirtyPlate>();
            isPlayerHoldingDirtyPlate = (dirtyPlate != null);
        }
    }
}
