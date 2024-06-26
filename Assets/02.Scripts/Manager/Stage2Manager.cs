using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Manager : MonoBehaviour
{
    public static Stage2Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


    }
    private void Start()
    {
        GameManager.Instance.CurrentStage = 2;
    }
}
