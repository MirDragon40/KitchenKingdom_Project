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
    public Slot[] playerSlots = new Slot[4];
    public GameObject[] CharacterSlots = new GameObject[4];

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
        foreach (var slot in CharacterSlots)
        {
            slot.SetActive(false);
        }

    }
    private void Start()
    {
        UpdatePlayerSlots();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePlayerSlots();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdatePlayerSlots();
    }

    public void UpdatePlayerSlots()
    {
        _playerCount = PhotonNetwork.PlayerList.Length;
        Debug.Log(_playerCount);

        for (int i = 0; i < playerSlots.Length; i++)
        {
            playerSlots[i].Nickname = string.Empty;
        }

        for (int i = 0; i < _playerCount; i++)
        {
            playerSlots[i].IsOccupied = true;
            playerSlots[i].Nickname = PhotonNetwork.PlayerList[i].NickName;
            CharacterSlots[i].SetActive(true);
        }

    }
}