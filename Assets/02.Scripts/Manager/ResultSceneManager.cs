
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
    public TextMeshProUGUI Total_Text;

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

        Review_Star1.SetActive(true);
        Review_Star2.SetActive(false);
        Review_Star3.SetActive(false);
        Review_Star4.SetActive(false);
        Review_Star5.SetActive(false);

        Stage1_Text.gameObject.SetActive(false);
        Stage2_Text.gameObject.SetActive(false);
        Stage3_Text.gameObject.SetActive(false);
        Total_Text.gameObject.SetActive(false);

    }

    private void Start()
    {

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

        Total_Text.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);
        Review_Anim.SetTrigger("Show_Review");
    }
}

