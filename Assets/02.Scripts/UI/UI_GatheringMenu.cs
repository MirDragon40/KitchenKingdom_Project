using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GatheringMenu : MonoBehaviour
{
    public GameObject UI_StartButton;

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UI_StartButton.SetActive(true);
        }
        else
        {
            UI_StartButton.SetActive(false);
        }
    }
}
