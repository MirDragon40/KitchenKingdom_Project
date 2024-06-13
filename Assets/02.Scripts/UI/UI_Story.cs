using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Story : MonoBehaviour
{
    // UI_News
    public GameObject NewsUI;

    public TextMeshProUGUI StoryText;
    public TextMeshProUGUI StoryText2;

    public Image SpeechBubbleImage_A;
    public Image SpeechBubbleImage_B;

    // UI_Man
    public GameObject ManUI;
    public TextMeshProUGUI StoryText3;

    private void Start()
    {
        StoryText.text = null;
        StoryText2.text = null;
        StoryText3.text = null;

        SpeechBubbleImage_A.gameObject.SetActive(false);
        SpeechBubbleImage_B.gameObject.SetActive(false);

        ManUI.gameObject.SetActive(false);
        
        StartCoroutine(Main_Coroutine());
    }
    
    private IEnumerator Main_Coroutine() 
    {
        yield return new WaitForSeconds(1f);
        First_Text();
        yield return new WaitForSeconds(2f);
        Second_Text();
        yield return new WaitForSeconds(5f);
        NewsUI.SetActive(false);
        ManUI.gameObject.SetActive(true);
        Third_Text();
    }

    private void First_Text() 
    {
        SpeechBubbleImage_A.gameObject.SetActive(true);
        string text = "최고의 레스토랑을\n뽑는 대회!";
        StartCoroutine(TypeText(StoryText, text));
    }
   
    private void Second_Text()
    {
        SpeechBubbleImage_B.gameObject.SetActive(true);
        string text = "7일 동안 진행하는 대회!\n우승하고 트로피를 차지하세요!";
        StartCoroutine(TypeText(StoryText2, text));
    }

    private void Third_Text() 
    {
        string text = "우아아아아아아아아아아ㅏ아아아아!!!";
        StartCoroutine(TypeText(StoryText3, text));
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
