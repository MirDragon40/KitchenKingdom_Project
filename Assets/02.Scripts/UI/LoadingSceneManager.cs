using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneNames 
{
    SubinScene,  // 임시
    YejinScene1, // 임시
}
public class LoadingSceneManager : MonoBehaviour
{
    public SceneNames NextScene;
    public Slider LoadingSliderUI;
    public TextMeshProUGUI LoadingText;

    private float _minLoadingTime = 2f;  // 최소 로딩 시간
    private float _loadingSpeed = 2f;    // 로딩 속도
    private float targetProgress = 0.9f; // 로딩 목표 진행률

    private void Start()
    {
        StartCoroutine(Loading_Coroutine());
    }

    public IEnumerator Loading_Coroutine()
    {
        // 로딩 시간 저장
        float startTime = Time.time;

        // 지정한 씬을 "비동기" 방식으로 로드한다.
        AsyncOperation ao = SceneManager.LoadSceneAsync((int)NextScene);  // 20초가 걸린다고 가정

        // 로드되는 씬의 모습이 화면에 보이지 않게 한다.
        ao.allowSceneActivation = false;

        // 로딩이 완료될 때까지... 반복
        while (!ao.isDone)
        {
            // 현재 진행률 0과 1사이 정규화
            float progressValue = Mathf.Clamp01(ao.progress / targetProgress);
            
            // 로딩 슬라이더 값 부드럽게 증가
            LoadingSliderUI.value = Mathf.Lerp(LoadingSliderUI.value, progressValue, _loadingSpeed * Time.deltaTime);

            // 로딩 진행률이 목표치 이상 && 최소 로딩 시간 지났는지
            if (ao.progress >= targetProgress && Time.time - startTime >= _minLoadingTime)
            {
                LoadingSliderUI.value = 1f;
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
