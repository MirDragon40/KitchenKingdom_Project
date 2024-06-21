using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class UI_Timer : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI TimerTextUI;
    public TextMeshProUGUI TimeOverTextUI;
    public Color TimerTextColor;

    private int _totalTime = 0;

    public GameObject SpeedUpFireUI;

    public Animator TimerAnimator;
    public Animator FireAnimator;
    public Animator TimeOverAnimator;

    public PhotonView PV;

    private void Awake()
    {
        
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();

        SpeedUpFireUI.gameObject.SetActive(false);
        StartCoroutine(TimerStart_Coroutine());
    }
    
    private IEnumerator TimerStart_Coroutine() 
    {
        yield return new WaitForSeconds(2f);
        if (PhotonNetwork.IsMasterClient)
        {
           // Debug.Log("aaa");
            _totalTime = 180;

            StartCoroutine(Timer_Coroution());

        }

    }
    private IEnumerator Timer_Coroution()
    {

        var wait = new WaitForSeconds(1f);

        while (true)
        {
            yield return wait;
            if (_totalTime == 30)
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
                PV.RPC(nameof(TimerEnded), RpcTarget.All);
                break;
            }
        }
    }

    [PunRPC]
    void ShowTimer(int number) 
    {
        //Debug.Log("aaa");

        int minutes = number / 60;
        int seconds = number % 60;

        //TimerTextUI.text = number.ToString();
        TimerTextUI.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    [PunRPC]
    void AnimationPlay() 
    {
        SpeedUpFireUI.gameObject.SetActive(true);

        TimerTextUI.color = TimerTextColor;

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
    */

    [PunRPC]
    public void TimerEnded()
    {
        TimeOverTextUI.gameObject.SetActive(true);

        TimeOverAnimator.SetTrigger("TimeOver");
    }
}
