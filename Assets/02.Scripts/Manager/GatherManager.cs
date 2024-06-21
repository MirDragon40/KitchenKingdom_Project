using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;



public struct Slot
{
    public string Nickname;
    public bool IsOccupied;
    public bool IsReady;
}

public class GatherManager : MonoBehaviourPunCallbacks
{
    private PhotonView _pv;
    private bool _isReady = false;
    private bool _isStartButton;
    public Button ReadyStartButton;

    public TMP_Text ButtonText;

    public static GatherManager Instance { get; private set; }
    public Slot[] playerSlots = new Slot[4];
    public TMP_Text[] NickNameSlots = new TMP_Text[4];
    public TMP_Text[] ReadyTexts = new TMP_Text[4];
    private int _readyPlayerCount = 0;
    public Dictionary<int, bool> _playerReadyStatus = new Dictionary<int, bool>();

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
        _pv = GetComponent<PhotonView>();
    }
    private void Start()
    {
        _pv.RPC("UpdatePlayerSlots", RpcTarget.All);
    }
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_playerCount == _readyPlayerCount + 1 || _playerCount == 1)
            {
                ButtonText.text = "Start";
                ReadyStartButton.interactable = true;
                _isStartButton = true;

            }
            else
            {
                ButtonText.text = "Waiting";
                ReadyStartButton.interactable = false;
                _isStartButton = false;
            }
        }
        else
        {
            ButtonText.text = "Ready";
            _isStartButton = false;

        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"{newPlayer.NickName}새 플레이어 입장");
        //UpdatePlayerSlots();
        _pv.RPC("UpdatePlayerSlots", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"{otherPlayer.NickName}플레이어 퇴장");
        _pv.RPC("UpdatePlayerSlots", RpcTarget.All);
    }

    [PunRPC]
    public void UpdatePlayerSlots()
    {
        _playerCount = PhotonNetwork.PlayerList.Length;

        for (int i = 0; i < playerSlots.Length; i++)
        {
            playerSlots[i].IsOccupied = false;
            playerSlots[i].Nickname = string.Empty;
            CharacterSlots[i].SetActive(false);
            ReadyTexts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _playerCount; i++)
        {
            playerSlots[i].IsOccupied = true;
            playerSlots[i].Nickname = PhotonNetwork.PlayerList[i].NickName;
            if (_playerReadyStatus.ContainsKey(PhotonNetwork.PlayerList[i].ActorNumber)) // ready여부
            {
                ReadyTexts[i].gameObject.SetActive(true);
            }
            else
            {
                ReadyTexts[i].gameObject.SetActive(false);
            }
            

            NickNameSlots[i].text = playerSlots[i].Nickname;
            CharacterSlots[i].SetActive(true);
        }
    }

    [PunRPC]
    public void PlayerReady(int actorNumber, bool readyState)
    {
        if (!_playerReadyStatus.ContainsKey(actorNumber))
        {
            _playerReadyStatus.Add(actorNumber, readyState);
        }
        else
        {
            _playerReadyStatus[actorNumber] = readyState;
        }

        _readyPlayerCount = 0;
        foreach (var playerStatus in _playerReadyStatus.Values)
        {
            if (playerStatus)
            {
                _readyPlayerCount++;
            }
        }
        Debug.Log($"readyPlayers : { _readyPlayerCount}");
        UpdatePlayerSlots();

    }
    public void OnReadyStartButtonClicked()
    {
        if (_isStartButton)
        {
            PhotonNetwork.LoadLevel("MainScene_Beta");
        }
        else
        {
            _pv.RPC("PlayerReady", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, true);
            ReadyStartButton.interactable = false;
        }
    }

}