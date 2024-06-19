using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WaitingText : MonoBehaviour
{
    public TextMeshProUGUI WaitingText;

    private void Start()
    {
        StartCoroutine(LoadingTextRepeat());
    }

    private IEnumerator LoadingTextRepeat()
    {
        while (true)
        {
            string text = "다른 플레이어를 기다리는 중...";
            yield return TypeText(WaitingText, text);
        }
    }

    private IEnumerator TypeText(TextMeshProUGUI textUI, string text)
    {
        textUI.text = string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            textUI.text += text[i];
            yield return new WaitForSeconds(0.1f);
        }
    }
}
