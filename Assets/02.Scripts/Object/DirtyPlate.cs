using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlate : MonoBehaviourPun
{
    public List<GameObject> DirtyPlates;
    public int DirtyPlateNum = 0;

    [HideInInspector]
    public PhotonView PhotonView;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();

        foreach (GameObject plate in DirtyPlates)
        {
            plate.SetActive(false);
        }
    }

    private void Update()
    {
        UpdatePlates();
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

    [PunRPC]
    public void SyncDirtyPlateState(int newDirtyPlateNum)
    {
        DirtyPlateNum = newDirtyPlateNum;
        UpdatePlates();
    }

    public void AddDirtyPlates(int amount)
    {
        DirtyPlateNum += amount;
        PhotonView.RPC(nameof(SyncDirtyPlateState), RpcTarget.AllBuffered, DirtyPlateNum);
    }

    public void RemoveDirtyPlate()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
