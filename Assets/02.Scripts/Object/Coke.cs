using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coke : IHoldable
{
    public GameObject CokeObject;

    public MeshRenderer CokeRenderer;
    public Material ThisCokeMaterial;
    public Material ChangeCokeMaterial;
    public GameObject CokeParticle;

    public override Vector3 DropOffset => new Vector3(-0.5f, 0f, 0f);


    private void Start()
    {
        CokeObject.gameObject.SetActive(false);
        CokeParticle.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            CokePour();
        }
    }

    // 콜라 컵이 생성되며 색 변경됨
    public void CokePour() 
    {
        CokeObject.gameObject.SetActive(true);
        StartCoroutine(CokePourCoroutine());
    }



    // 1초뒤에 파티클 생김 3초뒤에 파티클 없어짐 콜라 따라진 것처럼 보이게 (시간변경가능)
    private IEnumerator CokePourCoroutine() 
    {
        yield return new WaitForSeconds(1f);
        CokeParticle.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        CokeParticle.gameObject.SetActive(false);
        CokeRenderer.material = ChangeCokeMaterial;
    }





    public override void Hold(Character character, Transform handTransform)
    {
        _holdCharacter = character;

        // 각 아이템이 잡혔을 때 해줄 초기화 로직
        // 찾은 음식을 플레이어의 손 위치로 이동시킴
        transform.parent = handTransform;
        transform.localPosition = new Vector3(0, 0.4F, 0.5F);
        transform.localRotation = Quaternion.identity;
    } 

    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {
        // 저장한 위치와 회전으로 음식 배치
        transform.position = dropPosition;
        transform.rotation = dropRotation;


        transform.parent = null;
        //각 아이템이 떼어질 때 해줄 초기화 로직
        _holdCharacter = null;
    }

    public override void Place(Transform place)
    {
        transform.position = place.position;
        transform.rotation = place.rotation;
        //placeRotation = Quaternion.Euler(0, 0, 0);
        transform.parent = place;
        _holdCharacter = null;
    }
}
