using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneManager : MonoBehaviour
{
    public GameObject GameOverScreen;
    public GameObject LoseScreen;
    public GameObject VictoryScreen;

    private void Awake()
    {
        GameOverScreen.SetActive(false);
        LoseScreen.SetActive(false);
        VictoryScreen.SetActive(false);
    }

    private void Start()
    {
        if (GameManager.Instance.TotalScore <= 100)
        {
            GameOverScreen.SetActive(true);
            LoseScreen.SetActive(false);
            VictoryScreen.SetActive(false);
        }
        else if(GameManager.Instance.TotalScore <= 450)
        {
            GameOverScreen.SetActive(false);
            LoseScreen.SetActive(true);
            VictoryScreen.SetActive(false);
        }
        else if (GameManager.Instance.TotalScore > 450)
        {
            GameOverScreen.SetActive(false);
            LoseScreen.SetActive(false);
            VictoryScreen.SetActive(true);
        }
    }
}
