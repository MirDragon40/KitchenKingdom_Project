using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene_1 : MonoBehaviourPunCallbacks
{
   public static MainScene_1 Instance { get; private set; }

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



}
