using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CuttingAnimation : MonoBehaviour
{
    public Slider ProgressSlider;
    public GameObject FoodPrefab_Raw;
    public GameObject FoodPrefab_Cutting;
    public GameObject FoodPrefab_Cut;

    public GameObject currentFood;
    private Coroutine fillSliderCoroutine;
    private float elapsedTime;
    private bool _isAnimating = false;
    private bool _isFinalStage = false;

    public Transform PlacePosition;

    public OnChoppingBoard_Collider onChoppingBoard;

    private void Awake()
    {
        ProgressSlider.gameObject.SetActive(false);
        currentFood = onChoppingBoard.FoodOnBoard;

    }

    public void StartCuttingAnimation(float duration)
    {
        if (_isAnimating)
        {
            return;
        }

        if (fillSliderCoroutine != null)
        {
            StopCoroutine(fillSliderCoroutine);
        }

        fillSliderCoroutine = StartCoroutine(FillSliderOverTime(duration));
    }

    public void StopCuttingAnimation()
    {
        if (fillSliderCoroutine != null)
        {
            StopCoroutine(fillSliderCoroutine);
            fillSliderCoroutine = null;
        }

        ProgressSlider.value = 0f;
        ProgressSlider.gameObject.SetActive(false);
        _isAnimating = false;
        elapsedTime = 0f;
    }

    private IEnumerator FillSliderOverTime(float duration)
    {
        _isAnimating = true;
        ProgressSlider.gameObject.SetActive(true);
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            ProgressSlider.value = progress;

            if (progress >= 0.33f && progress < 0.66f && !_isFinalStage)
            {
                UpdateFoodPrefab(FoodPrefab_Cutting);
            }
            else if (progress >= 0.66f && !_isFinalStage)
            {
                UpdateFoodPrefab(FoodPrefab_Cut);
                _isFinalStage = true;
            }

            yield return null;
        }

        ProgressSlider.value = 1f;
        ProgressSlider.gameObject.SetActive(false);
        _isAnimating = false;
    }

    private void UpdateFoodPrefab(GameObject newPrefab)
    {
        // 도마 위에 음식이 없거나 썰기를 가능한 상태가 아닐 때 반환
        if (onChoppingBoard.FoodOnBoard == null || !onChoppingBoard.IsCuttable)
        {
            return;
        }

        if (currentFood != null)
        {
            Destroy(currentFood);
        }
        // FoodOnBoard 변수에 저장된 음식 오브젝트를 사용하여 새로운 프리팹을 생성
        currentFood = Instantiate(newPrefab, onChoppingBoard.FoodOnBoard.transform.position, PlacePosition.rotation);
    }
}

