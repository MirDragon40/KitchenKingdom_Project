using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class UI_Timer : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI TimerTextUI;

    private int _totalTime = 0;

    public GameObject SpeedUpFireUI;

    public Animator TimerAnimator;
    public Animator FireAnimator;

    public PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();

        SpeedUpFireUI.gameObject.SetActive(false);
        StartCoroutine(AAA());
    }
    
    private IEnumerator AAA() 
    {
        yield return new WaitForSeconds(2f);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("aaa");
            _totalTime = 60;

            StartCoroutine(Timer_Coroution());

        }

    }
    private IEnumerator Timer_Coroution()
    {

        var wait = new WaitForSeconds(1f);

        while (true)
        {
            yield return wait;
            if (_totalTime == 10)
            {
                PV.RPC("AnimationPlay", RpcTarget.All);
            }
            if (_totalTime > 0)
            {
                PV.RPC("ShowTimer", RpcTarget.All, _totalTime); //1초 마다 방 모두에게 전달
                _totalTime -= 1;

            }
            if (_totalTime <= 0)
            {
                PV.RPC("ShowTimer", RpcTarget.All, _totalTime); //1초 마다 방 모두에게 전달

                break;
            }
        }
    }

    [PunRPC]
    void ShowTimer(int number) 
    {
        Debug.Log("aaa");

        TimerTextUI.text = number.ToString();
    }
    [PunRPC]
    void AnimationPlay() 
    {
        SpeedUpFireUI.gameObject.SetActive(true);

        TimerAnimator.SetTrigger("SpeedUp");
        FireAnimator.SetTrigger("SpeedUp");
    }

    /*void StartTimer()
    {
        _currentTime = _totalTime;
        timerStarted = true;
    }

    void DisplayTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        string timeString = string.Format("{0:00} : {1:00}", minutes, seconds);
        TimerTextUI.text = timeString;
    }

    public void TimerEnded()
    {
        // 타이머 종료 시
        // 종료 시 씬 이동
    }*/
}
