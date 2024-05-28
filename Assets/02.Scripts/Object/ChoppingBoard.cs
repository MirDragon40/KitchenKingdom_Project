using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ChoppingBoard : MonoBehaviour
{
    public Animator Animator;
    public Slider ProgressSlider;

    private Coroutine fillSliderCoroutine;
    private float elapsedTime;

    private bool _isPossibleChopping = false;

    public Transform PlacePosition;

    private GameObject currentFood;

    public GameObject Item_Lettuce_Raw; // 처음 상태의 음식
    public GameObject Item_Lettuce_Cutting; // 중간 상태의 음식
    public GameObject Item_Lettuce_Cut; // 완전히 잘린 상태의 음식


    private void Awake()
    {
        ProgressSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isPossibleChopping )
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Animator.SetBool("Chopping", true);

                if (fillSliderCoroutine != null)
                {
                    StopCoroutine(fillSliderCoroutine);
                }

                fillSliderCoroutine = StartCoroutine(FillSliderOverTime(3.0f));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPossibleChopping = true;
        }
    }


    private void OnTriggerStay(Collider other)
    {

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
        }
    }

    private void UpdateFoodPrefab(GameObject newPrefab)
    {
        if (currentFood != null)
        {
            Destroy(currentFood);
        }
        currentFood = Instantiate(newPrefab, PlacePosition.position, PlacePosition.rotation, PlacePosition);
    }

    private IEnumerator FillSliderOverTime(float duration)
    {
        ProgressSlider.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {

            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            ProgressSlider.value = progress;


            // 음식 오브젝트 변경
            if (progress >= 0.33f && progress < 0.66f)
            {
                UpdateFoodPrefab(Item_Lettuce_Cutting);
            }
            else if (progress >= 0.66f)
            {
                UpdateFoodPrefab(Item_Lettuce_Cut);
            }


            yield return null;
        }

        ProgressSlider.value = 1f;
        ProgressSlider.gameObject.SetActive(false); // 3초가 다 지나면 슬라이더를 비활성화
        Animator.SetBool("Chopping", false);
        elapsedTime = 0f; // 초기화
    }

}

