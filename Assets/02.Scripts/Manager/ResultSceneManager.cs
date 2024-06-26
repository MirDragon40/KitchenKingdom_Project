using System.Collections;
using TMPro;
using UnityEngine;

public class ResultSceneManager : MonoBehaviour
{
    public static ResultSceneManager Instance { get; private set; }

    public GameObject Review_Star1;
    public GameObject Review_Star2;
    public GameObject Review_Star3;
    public GameObject Review_Star4;
    public GameObject Review_Star5;

    public Animator Review_Anim;
    public Animator ClipBoard_Anim;

    public TextMeshProUGUI Stage1_Text;
    public TextMeshProUGUI Stage2_Text;
    public TextMeshProUGUI Stage3_Text;
    public TextMeshProUGUI Stage4_Text;
    public TextMeshProUGUI Total_Text;

    public TextMeshProUGUI Stage1_Score_Text;
    public TextMeshProUGUI Stage2_Score_Text;
    public TextMeshProUGUI Stage3_Score_Text;
    public TextMeshProUGUI Stage4_Score_Text;
    public TextMeshProUGUI Total_Score_Text;

    private float _timeGap = 0.3f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 디버깅 코드 추가
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance가 null이다.");
        }

        if (Total_Score_Text == null)
        {
            Debug.LogError("Total_Score_Text가 null이다.");
        }

        // 기존 코드
        Review_Star1.SetActive(true);
        Review_Star2.SetActive(false);
        Review_Star3.SetActive(false);
        Review_Star4.SetActive(false);
        Review_Star5.SetActive(false);

        if (GameManager.Instance != null)
        {
            Stage1_Score_Text.text = $"{GameManager.Instance.StageScore[0]}";
            Stage2_Score_Text.text = $"{GameManager.Instance.StageScore[1]}";
            Stage3_Score_Text.text = $"{GameManager.Instance.StageScore[2]}";
            Stage4_Score_Text.text = $"{GameManager.Instance.StageScore[3]}";
            Total_Score_Text.text = $"{GameManager.Instance.TotalScore}";


            // 스테이지 마다 리뷰 결과 출력
            if(GameManager.Instance.StageScore[0] != 0 && GameManager.Instance.StageScore[1] == 0)  // 스테이지 1일때
            {

            }
            if(GameManager.Instance.StageScore[1] != 0 && GameManager.Instance.StageScore[2] == 0)  // 스테이지 2일때
            {

            }
            if (GameManager.Instance.StageScore[2] != 0 && GameManager.Instance.StageScore[3] == 0)  // 스테이지 3일때
            {

            }
            if(GameManager.Instance.StageScore[3] != 0)  // 스테이지 4일때
            {

            }
        }

      
    }

    private void Start()
    {

        Stage1_Text.gameObject.SetActive(false);
        Stage2_Text.gameObject.SetActive(false);
        Stage3_Text.gameObject.SetActive(false);
        Stage4_Text.gameObject.SetActive(false);
        
        Total_Text.gameObject.SetActive(false);


        StartCoroutine(ResultSceneAnimation_Coroutine());

       
    }


    private IEnumerator ResultSceneAnimation_Coroutine()
    {

        ClipBoard_Anim.SetTrigger("Show_Result");

        yield return new WaitForSeconds(1.5f);
        Stage1_Text.gameObject.SetActive(true);
        yield return new WaitForSeconds(_timeGap);

        Stage2_Text.gameObject.SetActive(true);
        yield return new WaitForSeconds(_timeGap);

        Stage3_Text.gameObject.SetActive(true);
        yield return new WaitForSeconds(_timeGap);

        Stage4_Text.gameObject.SetActive(true);
        yield return new WaitForSeconds(_timeGap);

        Total_Text.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);
        Review_Anim.SetTrigger("Show_Review");

        
    }
}

