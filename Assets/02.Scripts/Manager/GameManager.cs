using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
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

    public int[] StageScore = new int[4];
    public int TotalScore = 0;

    public GameObject OptionUl;
    private bool _optionUlOpen = false;

    public int CurrentStage { get; set; }


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
        CurrentStage = 2;

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

    public void ScoreInit()
    {
        if (CurrentStage == 1)
        {
            //StageScore[0] = OrderManager.Instance.sc
        }

        foreach (int score in StageScore)
        {
            TotalScore += score;
        }
        
    }
}
