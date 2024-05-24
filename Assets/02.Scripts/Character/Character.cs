using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character Instance { get; private set; }

    public PhotonView PhotonView { get; private set; }
    public CharacterHoldAbility HoldAbility;


    private void Start()
    {
        if (PhotonView.IsMine)
        {
            Instance = this;
        }

    }
}
