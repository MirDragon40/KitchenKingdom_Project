using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Ready,
    Go,
    TimeOver,
    Result,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State = GameState.Go; // 게임 상태, TimeScale관리
    private GameObject _spawnPoint;
    public Transform[] SpawnPoints = new Transform[4];

    public int[] StageScore = new int[4];
    public int TotalScore = 0;




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

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _spawnPoint = GameObject.Find("CharacterSpawnPoints");
        if (_spawnPoint != null)
        {
            for (int i = 0; i < SpawnPoints.Length; i++)
            {
                SpawnPoints[i] = _spawnPoint.transform.GetChild(i).transform;
            }
        }
        if (PhotonNetwork.IsConnected && _spawnPoint != null)
        {

            GameManager.Instance.SpawnPlayer();
        }
    }
    private void Update()
    {
        if (State == GameState.Ready)
        {
            Time.timeScale = 0;
        }
        else if (State == GameState.Go)
        {
            Time.timeScale = 1f;
        }



    }

    public void SpawnPlayer()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        int spawnIndex = actorNumber % SpawnPoints.Length - 1; // 각각 플레이어가 다른 곳에 spawn

        Vector3 spawnPosition = SpawnPoints[spawnIndex].position;
        Quaternion spawnRotation = SpawnPoints[spawnIndex].rotation;
        PhotonNetwork.Instantiate("Character1", spawnPosition, spawnRotation);
    }

    public void TotalScoreInit()
    {
        TotalScore = 0;
        foreach (int score in StageScore)
        {
            TotalScore += score;
        }

    }
}
