using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : MonoBehaviour
{
    public static Stage1Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


    }
    private void Start()
    {
        GameManager.Instance.CurrentStage = 1;
        if (PhotonNetwork.IsConnected)
        {

            GameManager.Instance.SpawnPlayer();
        }
    }

}
