using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knife : MonoBehaviour
{
    private Animator _animator;

    public Slider ProgressSlider;

    private Coroutine fillSliderCoroutine;
    private float elapsedTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        ProgressSlider.gameObject.SetActive(false);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                _animator.SetBool("Chopping",true);

                if (fillSliderCoroutine != null)
                {
                    StopCoroutine(fillSliderCoroutine);
                }

                fillSliderCoroutine = StartCoroutine(FillSliderOverTime(3.0f));
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("Chopping", false);

            if (fillSliderCoroutine != null)
            {
                StopCoroutine(fillSliderCoroutine);
            }

        }
    }



    private IEnumerator FillSliderOverTime(float duration)
    {
        ProgressSlider.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            ProgressSlider.value = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        ProgressSlider.value = 1f;
        ProgressSlider.gameObject.SetActive(false); // 3초가 다 지나면 슬라이더를 비활성화
        _animator.SetBool("Chopping", false);
        elapsedTime = 0f; // 초기화
    }
}

