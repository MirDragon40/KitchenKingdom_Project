using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Story : MonoBehaviour
{
    // 1
    public GameObject NewsUI;
    public TextMeshProUGUI StoryText;
    public TextMeshProUGUI StoryText2;
    public Image SpeechBubbleImage_A;
    public Image SpeechBubbleImage_B;

    // 2
    public Image ManImage;
    public Image ManImageBackgroundImage;
    public TextMeshProUGUI StoryText3;

    // FadeOut
    public Image FadeImage;
    public float FadeSpeed = 1.0f;

    private void Start()
    {
        Text_false();
        Image_false(); 
        StartCoroutine(Main_Coroutine());
    }

    private void Text_false() 
    {
        // 1
        StoryText.text = null;
        StoryText2.text = null;
         
        // 2
        //StoryText3.text = null;
    }
    private void Image_false() 
    {
        // 1
        SpeechBubbleImage_A.gameObject.SetActive(false);
        SpeechBubbleImage_B.gameObject.SetActive(false);

        // FadeOut
        FadeImage.gameObject.SetActive(false);
    }
    
    private IEnumerator Main_Coroutine() 
    {
        yield return new WaitForSeconds(1f);
        First_Text();
        yield return new WaitForSeconds(2f);
        Second_Text();
        yield return new WaitForSeconds(5f);
        FadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(2f);
        //FadeImage.gameObject.SetActive(false);
        //NewsUI.SetActive(false);
        //Third_Text();
        // 다음으로 갈 씬 넣기
        LoadingSceneManager.NextScene = SceneNames.Stage_1_Beta;
        SceneManager.LoadScene("LoadingScene");
    }

    // 1
    private void First_Text() 
    {
        SpeechBubbleImage_A.gameObject.SetActive(true);
        string text = "최고의 레스토랑을\n뽑는 대회!";
        StartCoroutine(TypeText(StoryText, text));
    }
   
    // 1
    private void Second_Text()
    {
        SpeechBubbleImage_B.gameObject.SetActive(true);
        string text = "4일 동안 진행하는 대회!\n우승하고 트로피를 차지하세요!";
        StartCoroutine(TypeText(StoryText2, text));
    }

    // 1~2 중간
    private IEnumerator FadeOut()
    {
        float alpha = 0.0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * FadeSpeed;
            FadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    // 이미지 투명도 조절
    private IEnumerator FadeOut_ManImage()
    {
        ManImage.gameObject.SetActive(true);

        float alpha = 0.0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * FadeSpeed;
            ManImage.color = new Color(ManImage.color.r, ManImage.color.g, ManImage.color.b, alpha);
            yield return null;
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
