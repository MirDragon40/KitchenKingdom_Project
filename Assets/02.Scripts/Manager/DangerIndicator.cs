using System.Collections;
using UnityEngine;

public class DangerIndicator : MonoBehaviour
{
    public SpriteRenderer danger;
    private Coroutine blinkingCoroutine;

    private void Awake()
    {
        // 게임 시작 시 경고창을 비활성화
        if (danger != null)
        {
            danger.enabled = false;
        }
    }

    public void ShowDangerIndicator(Sprite dangerSprite)
    {
        if (danger != null)
        {
            danger.sprite = dangerSprite;
            if (blinkingCoroutine == null)
            {
                blinkingCoroutine = StartCoroutine(Blinking());
            }
        }
    }
    public void HideDangerIndicator()
    {
        if (blinkingCoroutine != null)
        {
            StopCoroutine(blinkingCoroutine);
            blinkingCoroutine = null;
        }
        if (danger != null)
        {
            danger.enabled = false;

        }

    }

    private IEnumerator Blinking()
    {
        while (true)
        {
            danger.enabled = !danger.enabled;
            yield return new WaitForSeconds(0.2f);
        }
    }
}

