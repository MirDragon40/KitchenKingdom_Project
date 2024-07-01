using System.Collections;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

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
        TimeOverTextUI.gameObject.SetActive(false);
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();

        SpeedUpFireUI.gameObject.SetActive(false);
        StartCoroutine(TimerStart_Coroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9) && PhotonNetwork.IsMasterClient)
        {
            _totalTime = 3;
        }
    }
    private IEnumerator TimerStart_Coroutine()
    {
        yield return new WaitForSeconds(2f);
        if (PhotonNetwork.IsMasterClient)
        {
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
                _totalTime -= 1;
                PV.RPC("ShowTimer", RpcTarget.All, _totalTime); //1초 마다 방 모두에게 전달
            }
            if (_totalTime <= 0)
            {
                PV.RPC("ShowTimer", RpcTarget.All, _totalTime); //1초 마다 방 모두에게 전달
                PV.RPC("TimerEnded", RpcTarget.All);
                break;
            }
        }
    }

    [PunRPC]
    void ShowTimer(int number)
    {
        int minutes = number / 60;
        int seconds = number % 60;
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

    [PunRPC]
    void TimerEnded()
    {
        Debug.Log("TimerEnded 함수 실행");
        TimeOverTextUI.gameObject.SetActive(true);
        TimeOverAnimator.SetTrigger("TimeOver");
        GameManager.Instance.TotalScoreInit();
        StartCoroutine(StopGameAfterAnimation());
    }

    private IEnumerator StopGameAfterAnimation()
    {
        yield return new WaitForSeconds(TimeOverAnimator.GetCurrentAnimatorStateInfo(0).length);
        Time.timeScale = 0f;

        // 디버그 로그 추가
        Debug.Log("Time.timeScale을 0으로 설정.");

        // Reset time scale before scene change
        yield return new WaitForSecondsRealtime(3f);  // Use WaitForSecondsRealtime to ensure the wait is in real-time
        Time.timeScale = 1f;

        // 디버그 로그 추가
        Debug.Log("씬 전환 시도 직전. Time.timeScale을 1로 설정.");

        // Load the scene by name
        if (PhotonNetwork.IsMasterClient)
        {

            
                PhotonNetwork.LoadLevel("ResultScene");
            
        }    

        // 디버그 로그 추가
        Debug.Log("씬 전환을 시도.");
    }
}
