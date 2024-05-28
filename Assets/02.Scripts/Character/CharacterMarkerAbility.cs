using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CharacterMarkerAbility : MonoBehaviour
{
    public TMP_Text NameText;
    private PhotonView _pv;
    public GameObject MarkerCircle;
    private Transform _initialLocalTransform;




    void Start()
    {
        _pv = GetComponent<PhotonView>();

        if (_pv.IsMine)
        {
            MarkerCircle.SetActive(true);
            _pv.RPC("SetNickName", RpcTarget.All, PhotonNetwork.NickName);
        }
        else
        {
            MarkerCircle.SetActive(false);
        }
    }
    [PunRPC]
    public void SetNickName(string nickName)
    {
        NameText.text = nickName;
    }


}
