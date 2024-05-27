using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coke : MonoBehaviour
{
    public GameObject CokeObject;

    public MeshRenderer CokeRenderer;
    public Material ThisCokeMaterial;
    public Material ChangeCokeMaterial;
    public GameObject CokeParticle;

    private void Start()
    {
        CokeObject.gameObject.SetActive(false);
        CokeParticle.gameObject.SetActive(false);
    }

    private void Update()
    {
        /*if(Input.GetKeyUp(KeyCode.Escape)) 
        {
            CokePour();
        }*/
    }

    // 콜라 컵이 생성되며 색 변경됨
    public void CokePour() 
    {
        CokeObject.gameObject.SetActive(true);
        StartCoroutine(CokePourCoroutine());
    }

    // 1초뒤에 파티클 생김 3초뒤에 파티클 없어짐 콜라 따라진 것처럼 보이게
    private IEnumerator CokePourCoroutine() 
    {
        yield return new WaitForSeconds(1f);
        CokeParticle.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        CokeParticle.gameObject.SetActive(false);
        CokeRenderer.material = ChangeCokeMaterial;
    }
}
