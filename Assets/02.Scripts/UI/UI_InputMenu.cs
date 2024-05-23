using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_InputMenu : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public TMP_InputField MyInputField;
    public GameObject GatheringMenu;
    public CinemachineVirtualCamera NameInputCamera;


    private void Awake()
    {
        if (MyInputField == null)
        {
            MyInputField = GetComponentInChildren<TMP_InputField>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (MyInputField.text.Length > 0 )
            { 
                HandleSubmit(MyInputField.text);
            }
        }
    }
    private void HandleSubmit(string input)
    {
        PhotonNetwork.NickName = input;
        NameInputCamera.Priority = 1;
        GatheringMenu.gameObject.SetActive(true);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4; // 입장가능한 최대 플레이어수
        roomOptions.IsVisible = true; // 로비에서 방 목록에 노출할 것인가?
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom("PlayRoom", roomOptions, TypedLobby.Default);
        this.gameObject.SetActive(false);

    }
}
