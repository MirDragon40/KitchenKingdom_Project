using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    CutScene,
    Go,
    Pause,
    Over,
    Ending
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State = GameState.Go; // 게임 상태, TimeScale관리
    public Transform[] SpawnPoints= new Transform[4];
    public int TotalScore = 0;
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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }
    private void SpawnPlayer()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        int spawnIndex = actorNumber % SpawnPoints.Length; // 각각 플레이어가 다른 곳에 spawn

        Vector3 spawnPosition = SpawnPoints[spawnIndex].position;
        Quaternion spawnRotation = SpawnPoints[spawnIndex].rotation;
        PhotonNetwork.Instantiate("Character1", spawnPosition, spawnRotation);

    }



}
