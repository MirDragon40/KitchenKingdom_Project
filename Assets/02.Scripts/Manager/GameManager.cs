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

    public int[] StageScore = new int[7];

    public int Stage1_Score = 0;
    public int Stage2_Score = 0;
    public int Stage3_Score = 0;
    public int Stage4_Score = 0;
    public int TotalScore = 0;

    public GameObject OptionUl;
    private bool _optionUlOpen = false;

    public int CurrentStage { get; private set; }


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
        CurrentStage = 1;

        DontDestroyOnLoad(gameObject);
        OptionUl.gameObject.SetActive(false);
    }

    private void Update()
    {
        // 옵션창 켜고 끄기
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (_optionUlOpen)
            {
                OptionUl.gameObject.SetActive(false);
                _optionUlOpen = false;
            }
            else 
            {
                OptionUl.gameObject.SetActive(true);
                _optionUlOpen = true;
            }
        }

        TotalScore  = Stage1_Score + Stage2_Score + Stage3_Score + Stage4_Score;
    }

    private void Start()
    {
/*        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }*/
    }

    public void SpawnPlayer()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        int spawnIndex = actorNumber % SpawnPoints.Length -1; // 각각 플레이어가 다른 곳에 spawn

        Vector3 spawnPosition = SpawnPoints[spawnIndex].position;
        Quaternion spawnRotation = SpawnPoints[spawnIndex].rotation;
        PhotonNetwork.Instantiate("Character1", spawnPosition, spawnRotation);
    }
}
