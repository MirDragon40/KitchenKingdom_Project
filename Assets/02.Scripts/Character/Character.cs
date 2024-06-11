using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public PhotonView PhotonView { get; private set; }
    public CharacterHoldAbility HoldAbility;
    private void Awake()
    {
        HoldAbility = GetComponent<CharacterHoldAbility>();
        PhotonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
/*        if (PhotonView.IsMine)
        {
            Instance = this;
        }*/

    }
}
