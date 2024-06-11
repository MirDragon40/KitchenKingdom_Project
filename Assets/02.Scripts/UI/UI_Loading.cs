using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Loading : MonoBehaviour
{
    public TextMeshProUGUI LoadingText;

    private void Start()
    {
        StartCoroutine(LoadingTextRepeat());
    }

    private IEnumerator LoadingTextRepeat() 
    {
        while (true) 
        {
            string text = "일하러 가는 중 ...";
            yield return TypeText(LoadingText, text);
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
