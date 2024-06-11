using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class UI_Timer : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI TimerTextUI;

    private float _totalTime = 0f;

    private float _currentTime;

    private bool timerStarted = false;

    public GameObject SpeedUpFireUI;

    public Animator TimerAnimator;
    public Animator FireAnimator;

    public PhotonView PV;

    private void Start()
    {
        SpeedUpFireUI.gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            _totalTime = 60f;

            StartCoroutine(Timer_Coroution());

        }
    }
    private void Update()
    {
        /*if (timerStarted)
        {
            // 시간 업데이트
            _currentTime -= Time.deltaTime;

            // UI에 시간 표시
            DisplayTime(_currentTime);

            // 30초가 남으면 불 애니메이션 나옴
            if (_currentTime <= 175f) // 테스트 위해 5초뒤로.. 30초로 변경하기
            {
                SpeedUpFireUI.gameObject.SetActive(true);

                TimerAnimator.SetTrigger("SpeedUp");
                FireAnimator.SetTrigger("SpeedUp");
            }

            // 타이머가 종료되었을 때 처리
            if (_currentTime <= 0f)
            {
                TimerEnded();
            }
        }*/

        //Debug.Log(_totalTime);
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
