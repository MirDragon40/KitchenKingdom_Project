using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlateStand : MonoBehaviour
{
    public List<GameObject> DirtyPlates;
    public int DirtyPlateNum = 5;

    private CharacterHoldAbility characterHoldAbility;
    private DirtyPlate dirtyPlate;

    private bool isPlayerInTrigger = false;
    private bool isPlayerHoldingDirtyPlate = false;

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
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space))
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
        if (other.CompareTag("Player"))
        {
            characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
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
        }
    }

    private void GiveDirtyPlates()
    {
        if (DirtyPlateNum > 0 && characterHoldAbility != null)
        {
            characterHoldAbility.SpawnDirtyPlateOnHand();

            // Ensure dirtyPlate is updated after spawning a new plate
            UpdateDirtyPlateStatus();

            // 다시 dirtyPlate를 확인하고 업데이트합니다.
            dirtyPlate = characterHoldAbility.gameObject.GetComponentInChildren<DirtyPlate>();
            if (dirtyPlate != null)
            {
                dirtyPlate.DirtyPlateNum += DirtyPlateNum;
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
