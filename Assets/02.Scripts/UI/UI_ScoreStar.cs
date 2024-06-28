using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ScoreStar : MonoBehaviour
{
    public TMP_Text ScoreText;

    void Start()
    {
        
    }
    void Update()
    {
        if(GameManager.Instance.CurrentStage == 1)
        {
            ScoreText.text = $"{GameManager.Instance.StageScore[0]} / 100";
        }
        else if (GameManager.Instance.CurrentStage == 2)
        {
            ScoreText.text = $"{GameManager.Instance.StageScore[1]} / 100";
        }
        else if (GameManager.Instance.CurrentStage == 3)
        {
            ScoreText.text = $"{GameManager.Instance.StageScore[2]} / 100";
        }
        else if (GameManager.Instance.CurrentStage == 4)
        {
            ScoreText.text = $"{GameManager.Instance.StageScore[3]} / 100";
        }
    }
}
