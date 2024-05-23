using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene_1 : MonoBehaviourPunCallbacks
{
   public static MainScene_1 Instance { get; private set; }

    private bool _init = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (!_init)
        {
            Init();
        }
    }

    public void Init()
    {
        _init = true;
        PhotonNetwork.Instantiate($"Character", Vector3.zero, Quaternion.identity);

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }



    }

}
