using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BilgeScrollView : MonoBehaviour
{
    public GameObject content; // Scroll View의 Content 오브젝트
    public GameObject itemPrefab; // 추가할 아이템의 프리팹
    public int maxItems = 5; // 최대 아이템 개수 제한

    public HorizontalLayoutGroup _horizontalLayoutGroup;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AddItem();
        }
    }

    // 아이템을 추가하는 함수
    public GameObject AddItem()
    {
            GameObject newItem = Instantiate(itemPrefab, content.transform);
            AnimateItem(newItem);
            return newItem;
    }


    // 프리팹을 일정 시간 간격으로 추가하는 Coroutine
    private IEnumerator AddItemPrefab()
    {
        while (true)
        {
            yield return new WaitForSeconds(5); // 5초 대기
            AddItem(); // 아이템 추가
        }
    }

    // 아이템 추가 시 애니메이션 적용
    private void AnimateItem(GameObject item)
    {
        _horizontalLayoutGroup.enabled = false;

        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 100); // 시작 위치 (원하는 시작 위치로 설정)
        rectTransform.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBounce); // 0.5초 동안 y좌표를 0으로 애니메이션 (Ease 효과 적용)

        CanvasGroup canvasGroup = item.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f); // 0.5초 동안 투명도 애니메이션
        StartCoroutine(HorizontalLayoutGroupON());
    }

    private IEnumerator HorizontalLayoutGroupON()
    {
        yield return new WaitForSeconds(0.01f);
        _horizontalLayoutGroup.enabled = true;
    }

    private IEnumerator StartAddItem() 
    {
        yield return new WaitForSeconds(0.5f);
        AddItem();
    }
}
