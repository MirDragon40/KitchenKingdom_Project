using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ready_Start_Font : MonoBehaviour
{
    public GameObject ReadyFont;
    public GameObject StartFont;

    private void Start()
    {
        StartFont.gameObject.SetActive(false);
        StartCoroutine(ReadyFont_false());
    }
    private IEnumerator ReadyFont_false()
    {
        yield return new WaitForSeconds(1.5f);
        ReadyFont.gameObject.SetActive(false);
        StartFont.gameObject.SetActive(true);
    }
}
