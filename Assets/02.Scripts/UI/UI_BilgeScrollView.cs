using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BilgeScrollView : MonoBehaviour
{
    public GameObject content; // Scroll View의 Content 오브젝트
    public UI_Bilge itemPrefab; // 추가할 아이템의 프리팹
    public int maxItems = 5; // 최대 아이템 개수 제한
    public List<UI_Bilge> OrderBills = new List<UI_Bilge>();

    public HorizontalLayoutGroup _horizontalLayoutGroup;


    private void Update()
    {

    }

    public void RefreshView()
    {

    }

    public void RemoveItem(string orderName)
    {
        int index = OrderBills.FindIndex(item => item.OrderedFood == orderName);

        Debug.Log("전체 개수:" + OrderBills.Count);
        Debug.Log("찾은 위치:" + index);

        if(index >= 0)
        {
            Debug.Log(index);
            Destroy(OrderBills[index].gameObject);
            OrderBills.RemoveAt(index);

        }
    }

    // 아이템을 추가하는 함수
    public UI_Bilge AddItem()
    {
        UI_Bilge newItem = Instantiate< UI_Bilge>(itemPrefab, content.transform);
        OrderBills.Add(newItem);

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
    private void AnimateItem(UI_Bilge item)
    {
        _horizontalLayoutGroup.enabled = false;

        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 100); // 시작 위치 (원하는 시작 위치로 설정)
        rectTransform.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBounce); // 0.5초 동안 y좌표를 0으로 애니메이션 (Ease 효과 적용)

        CanvasGroup canvasGroup = item.gameObject.AddComponent<CanvasGroup>();
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
