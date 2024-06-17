using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlateStand : MonoBehaviourPun
{
    public List<GameObject> DirtyPlates;
    public int DirtyPlateNum = 5;

    private CharacterHoldAbility characterHoldAbility;
    private Character _nearbyCharacter;

    private DirtyPlate dirtyPlate;

    private bool isPlayerInTrigger = false;
    private bool isPlayerHoldingDirtyPlate = false;
    private bool isPlayerNearby => _nearbyCharacter != null;

    private PhotonView _pv;

    private void Awake()
    {
        foreach (GameObject plate in DirtyPlates)
        {
            plate.SetActive(false);
        }

        _pv = GetComponent<PhotonView>();
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
            GiveDirtyPlates();
        }
    }

    private void UpdatePlates()
    {
        for (int i = 0; i < DirtyPlates.Count; i++)
        {
            DirtyPlates[i].SetActive(i < DirtyPlateNum);
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
        if (DirtyPlateNum > 0 && _nearbyCharacter != null)
        {
            UpdateDirtyPlateStatus();

            if (isPlayerHoldingDirtyPlate)
            {
                dirtyPlate.DirtyPlateNum += DirtyPlateNum;
            }
            else
            {
                _nearbyCharacter.PhotonView.RPC("RequestSpawnDirtyPlateOnHand", RpcTarget.MasterClient);
                dirtyPlate = characterHoldAbility.gameObject.GetComponentInChildren<DirtyPlate>();

                if (dirtyPlate != null)
                {
                    dirtyPlate.DirtyPlateNum = DirtyPlateNum;
                }
            }

            _pv.RPC(nameof(UpdatePlateNum), RpcTarget.AllBuffered, 0);
            UpdatePlates();
        }
    }

    [PunRPC]
    private void UpdatePlateNum(int newPlateNum)
    {
        DirtyPlateNum = newPlateNum;
        UpdatePlates();
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
