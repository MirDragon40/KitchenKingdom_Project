using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_News : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public TextMeshProUGUI Text2;
    public Image Image;
    public Image Image2;

    private void Start()
    {
        Text.text = null;
        Text2.text = null;
        Image.gameObject.SetActive(false);
        Image2.gameObject.SetActive(false);

        StartCoroutine(TextTime());
        StartCoroutine(TextTime2());
    }

    private IEnumerator TextTime() 
    {
        yield return new WaitForSeconds(2f);
        Image.gameObject.SetActive(true);
        string text = "수비닝~~~ \n 바부~~~~~ \n  메롱~~~~~~";
        StartCoroutine(TypeText(Text, text));
    }

    private IEnumerator TextTime2() 
    {
        yield return new WaitForSeconds(7f);
        Image2.gameObject.SetActive(true);
        string text = "흐어어엉ㅇ어 ㅠㅠㅠㅠ\n집가고싶엉으으어ㅠㅠㅠ";
        StartCoroutine(TypeText(Text2, text));
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
