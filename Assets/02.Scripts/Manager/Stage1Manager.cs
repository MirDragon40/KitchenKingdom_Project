using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : MonoBehaviour
{
    public static Stage1Manager Instance {  get; private set; }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        GameManager.Instance.CurrentStage = 1;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
