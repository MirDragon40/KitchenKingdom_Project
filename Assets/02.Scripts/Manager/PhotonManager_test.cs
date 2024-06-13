using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhotonManager_test : MonoBehaviourPunCallbacks
{


    private void Awake()
    {
       // PhotonNetwork.AutomaticallySyncScene = true;
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
        int randomIndex = Random.Range(0, 100);
        PhotonNetwork.JoinOrCreateRoom($"PlayRoom564", roomOptions, TypedLobby.Default);

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
        // GatherManager.Instance.UpdatePlayerSlots();
        GameManager.Instance.SpawnPlayer();

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

}
