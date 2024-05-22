using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class Slot
{
    public string Nickname;
    public bool IsOccupied;
}

public class GatherManager : MonoBehaviourPunCallbacks
{
    public static GatherManager Instance { get; private set; }
    public Slot[] playerSlots = new Slot[4]; // 4개의 Text UI 요소를 연결
    private int _playerCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    private void Start()
    {
        UpdatePlayerSlots();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        UpdatePlayerSlots();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        _playerCount++;
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePlayerSlots();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        _playerCount--;
        base.OnPlayerLeftRoom(otherPlayer);
        UpdatePlayerSlots();
    }

    void UpdatePlayerSlots()
    {
        _playerCount = PhotonNetwork.PlayerList.Length;
        for (int i = 0; i < _playerCount; i++)
        {
            playerSlots[i].Nickname = string.Empty;
        }

        for (int i = 0; i < _playerCount; i++)
        {
            if (playerSlots[i].IsOccupied)
            {
                playerSlots[i].Nickname = PhotonNetwork.PlayerList[i].NickName;
            }
        }
    }
}