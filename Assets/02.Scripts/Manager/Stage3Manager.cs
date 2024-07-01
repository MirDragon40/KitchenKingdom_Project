using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Stage3Manager : MonoBehaviour
{
    public static Stage3Manager Instance { get; private set; }
    public GameObject OptionUl;
    private bool _optionUlOpen = false;

    public Image ControlImage;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


    }
    private void Start()
    {
        GameManager.Instance.CurrentStage = 3;
    }
    private void Update()
    {
        // 옵션창 켜고 끄기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_optionUlOpen)
            {
                //OptionUl.gameObject.SetActive(false);
                _optionUlOpen = false;
            }
            else
            {
                OptionUl.gameObject.SetActive(true);
                ControlImage.gameObject.SetActive(false);
                _optionUlOpen = true;
            }

        }
    }
}
