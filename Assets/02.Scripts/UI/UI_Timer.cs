using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Timer : MonoBehaviour
{
    public TextMeshProUGUI TimerTextUI;

    private float _totalTime = 180f;

    private float _currentTime;

    private bool timerStarted = false;

    public GameObject SpeedUpFireUI;

    public Animator TimerAnimator;
    public Animator FireAnimator;

    private void Start()
    {
        SpeedUpFireUI.gameObject.SetActive(false);
        StartTimer();
    }
    private void Update()
    {
        if (timerStarted)
        {
            // 시간 업데이트
            _currentTime -= Time.deltaTime;

            // UI에 시간 표시
            DisplayTime(_currentTime);

            // 30초가 남으면 불 애니메이션 나옴
            if (_currentTime <= 30f) 
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
        }
    }

    void StartTimer()
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
    }
}
