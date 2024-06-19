using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ready_Start_Text : MonoBehaviour
{
    public GameObject ReadyText;
    public GameObject StartText;

    private void Start()
    {
        StartText.gameObject.SetActive(false);
        ReadyText.gameObject.SetActive(false);
        StartCoroutine(ReadyFont_false());
    }
    private IEnumerator ReadyFont_false()
    {
        yield return new WaitForSeconds(0.5f);
        ReadyText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        ReadyText.gameObject.SetActive(false);
        StartText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        StartText.gameObject.SetActive(false);
    }
}
