using System.Collections;
using UnityEngine;

public class ChoppingBoard : MonoBehaviour
{
    public Animator Animator;

    private Coroutine fillSliderCoroutine;
    private float elapsedTime;

    private bool _isPossibleChopping = false;

    public Transform PlacePosition;
    public CuttingAnimation cuttingAnimation; // CuttingAnimation 스크립트를 참조하는 변수 추가

    public OnChoppingBoard_Collider onChoppingBoard;

    private void Awake()
    {
        // 슬라이더 관련 초기화 제거
    }

    private void Update()
    {
        if (_isPossibleChopping)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Animator.SetBool("Chopping", true);

                if (fillSliderCoroutine != null)
                {
                    StopCoroutine(fillSliderCoroutine);
                }

                fillSliderCoroutine = StartCoroutine(FillSliderOverTime(3.0f));
                cuttingAnimation.StartCuttingAnimation(3.0f); // CuttingAnimation의 슬라이더 애니메이션 시작
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && onChoppingBoard.FoodOnBoard != null )
        {
            _isPossibleChopping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _isPossibleChopping)
        {
            Animator.SetBool("Chopping", false);
            _isPossibleChopping = false;

            if (fillSliderCoroutine != null)
            {
                StopCoroutine(fillSliderCoroutine);
            }
            cuttingAnimation.StopCuttingAnimation(); // CuttingAnimation의 슬라이더 애니메이션 중지
        }
    }
    
    private IEnumerator FillSliderOverTime(float duration)
    {
        // 슬라이더 관련 로직 제거

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Animator.SetBool("Chopping", false);
        elapsedTime = 0f; // 초기화
    }
}
