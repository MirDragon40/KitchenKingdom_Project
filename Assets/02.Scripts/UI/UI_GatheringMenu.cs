using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GatheringMenu : MonoBehaviour
{
    public GameObject UI_StartButton;
    public void ReadyButtonClicked()
    {
        GatherManager.Instance.OnReadyButtonClicked();
    }
    private void Update()
    {
        if (GatherManager.Instance.IsAllReady)
        {
            UI_StartButton.SetActive(true);
        }
        else
        {
            UI_StartButton.SetActive(false);
        }
    }
}
