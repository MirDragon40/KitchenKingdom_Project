using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PhotonManager : MonoBehaviourPunCallbacks //PUN의 다양한 서버 이벤트(콜백 함수)를 받는다.
{
    public static PhotonManager Instance { get; private set; }
    public Button startButton;
    public TMP_Text startBtnText;


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

    private void Start()
    {
        // 연결을 하고싶다.
        // 순서
        // 1. 게임 버전을 설정한다.
        PhotonNetwork.GameVersion = "0.0.1";

        // 2. 닉네임을 설정한다.
         PhotonNetwork.NickName = $"Player_{UnityEngine.Random.Range(0, 100)}";
        // 3. 씬을 설정한다.
        // 4. 연결한다.
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.SendRate = 50;
        PhotonNetwork.SerializationRate = 30;
    }
    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnected()
    {
        Debug.Log("(name)서버 접속 성공");
        Debug.Log(PhotonNetwork.CloudRegion);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("서버 연결 해제");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터서버 연결");
        Debug.Log($"In Lobby? : {PhotonNetwork.InLobby}");

        // 기본 호텔의 로비에 들어가겠다.
        // 로비:매치메이킹(방 목록, 방 생성, 방 입장)

        PhotonNetwork.JoinLobby(TypedLobby.Default); // parameter 입력하지 않으면 TypedLobby.default

    }
    // 로비에 접속한 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"In Lobby? : {PhotonNetwork.InLobby}");
        Debug.Log("로비에 입장하였습니다");

        string nickname = PhotonNetwork.NickName;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4; // 입장가능한 최대 플레이어수
        roomOptions.IsVisible = true; // 로비에서 방 목록에 노출할 것인가?
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom("PlayRoom",roomOptions, TypedLobby.Default);

    }
    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 성공!");
        Debug.Log($"RoomName: {PhotonNetwork.CurrentRoom.Name}");
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("방 join 성공!");
        Debug.Log($"RoomName: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"Current Players: {PhotonNetwork.CurrentRoom.PlayerCount}");
        int sizeOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        AssignTeam(sizeOfPlayers);
        if (PhotonNetwork.IsMasterClient)//만약 내가 마스터 클라이언트라면
        {
            startBtnText.text = "waiting for players";
        }
        else
        {
            startBtnText.text = "Ready!";
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("방 생성 실패!");
        Debug.Log(message);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("랜덤방 생성 실패!");
        Debug.Log(message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Join 실패!");
    }
    void AssignTeam(int sizeOfPlayer)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        if (sizeOfPlayer % 2 == 0)
        {
            hash.Add("Team", 0);
        }
        else
        {
            hash.Add("Team", 1);
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public void OnClickStartButton()
    {
        if (!PhotonNetwork.IsMasterClient)//방장이 아닌 참가자라면
        {
        //    SendMsg();
            startButton.interactable = false;//한번 레디 누르면 못 바꾼다
            startBtnText.text = "Wait...";
        }
        else
        {
            if (_count == 4)//모든 플레이어가(4명) 준비 완료되었다면
            {
          //      lobbyText.text = "All Set : Play the Game Scene";
                PhotonNetwork.LoadLevel(1);//1번째 씬을 불러온다
            }
        }
    }
    private void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    private enum EventCodes
    {
        ready = 1
    }
    private int _count = 1;
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        object content = photonEvent.CustomData;
        EventCodes code = (EventCodes)eventCode;
        if (code == EventCodes.ready)//받은 이벤트의 분류가 ready라면
        {
            object[] datas = content as object[];
            if (PhotonNetwork.IsMasterClient)//내가 마스터클라이언트라면
            {
                _count++;
                if (_count == 4)//모두 준비완료가 되었다면
                {
                    startBtnText.text = "START !";
                }
                else
                {
                    startBtnText.text = "Only " + _count + " / 4 players are Ready";
                }
            }
        }
    }
}
