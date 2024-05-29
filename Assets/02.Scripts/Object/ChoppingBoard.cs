using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChoppingBoard : CookStand
{
    public Slider ChoppingProgressSlider;
    public Animator Animator;
    private Coroutine fillSliderCoroutine;
    private bool _isPossibleChopping = false;
    public OnChoppingBoard_Collider onChoppingBoard;

    private void Awake()
    {
        ChoppingProgressSlider.value = 0f; // 슬라이더 초기화
    }

    private void Update()
    {
        if (_isPossibleChopping && onChoppingBoard.FoodObject != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Animator.SetBool("Chopping", true);

                if (fillSliderCoroutine != null)
                {
                    StopCoroutine(fillSliderCoroutine);
                }

                onChoppingBoard.FoodObject.StartCooking();
                fillSliderCoroutine = StartCoroutine(FillSliderOverTime(3.0f));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && onChoppingBoard.FoodOnBoard != null)
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

            if (onChoppingBoard.FoodObject != null)
            {
                onChoppingBoard.FoodObject.StopCooking();
            }
        }
    }

    private IEnumerator FillSliderOverTime(float duration)
    {
        FoodObject foodObject = onChoppingBoard.FoodObject;

        if (foodObject == null)
            yield break;

        float startProgress = foodObject.CookProgress;
        float elapsedTime = startProgress * duration;

        while (elapsedTime < duration)
        {
            if (!foodObject.IsCooking)
            {
                yield return null;
                continue;
            }

            elapsedTime += Time.deltaTime;
            foodObject.CookProgress = Mathf.Clamp01(elapsedTime / duration);
            ChoppingProgressSlider.value = foodObject.CookProgress;

            if (foodObject.CookProgress >= 1f)
            {
                break;
            }

            yield return null;
        }

        Animator.SetBool("Chopping", false);
        foodObject.StopCooking();
    }
}
