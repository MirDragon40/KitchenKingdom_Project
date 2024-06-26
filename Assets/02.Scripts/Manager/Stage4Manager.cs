using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4Manager : MonoBehaviour
{
    public static Stage4Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


    }
    private void Start()
    {
        GameManager.Instance.CurrentStage = 4;
    }
}
