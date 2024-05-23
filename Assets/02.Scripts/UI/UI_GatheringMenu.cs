using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_GatheringMenu : MonoBehaviour
{
    private int _readyPlayerCount = 0;
    //public Button ReadyStartButton;
    private bool _isStartButton;
    public TMP_Text ButtonText;
    private bool _isReady = false;



    private void Update()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                ButtonText.text = "게임 시작";
                _isStartButton = true;
            }
            else
            {
                ButtonText.text = "대기중..";
                _isStartButton = false;
            }
        }
        else
        {
            ButtonText.text = "Ready";
            _isStartButton = false;

        }
    }
}
