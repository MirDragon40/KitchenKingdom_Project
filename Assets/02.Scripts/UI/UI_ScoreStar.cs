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
        ScoreText.text = $"{GameManager.Instance.TotalScore} / 100";
    }
}
