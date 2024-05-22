using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GatheringMenu : MonoBehaviour
{
    public TMP_Text ButtonText;

    
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                ButtonText.text = "게임 시작";
            }

        }
        else
        {
            ButtonText.text = "Ready";

        }
    }
}
