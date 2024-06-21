using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coke : IHoldable
{
    public GameObject CokeObject;
    public PhotonView PV;
    public MeshRenderer CokeRenderer;
    public Material ThisCokeMaterial;
    public Material ChangeCokeMaterial;
    public GameObject CokeParticle;

    public Collider CokeCollider;

    public override Vector3 DropOffset => new Vector3(-0.5f, 0f, 0f);
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        CokeCollider.enabled = false;
    }
    private void Start()
    {
        CokePour();
    }
   
    // 콜라 컵이 생성되며 색 변경됨
    public void CokePour() 
    {
        StartCoroutine(CokePourCoroutine());
    }

    // 1초뒤에 파티클 생김 3초뒤에 파티클 없어짐 -> 콜라 따라진 것처럼 보이게 (시간변경가능)
    private IEnumerator CokePourCoroutine() 
    {
        CokeCollider.enabled = false;

        CokeParticle.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        CokeParticle.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);
        CokeParticle.gameObject.SetActive(false);

        CokeRenderer.material = ChangeCokeMaterial;
        yield return new WaitForSeconds(0.5f);
        CokeCollider.enabled = true;
    }

    public override void Hold(Character character, Transform handTransform)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (PV.OwnerActorNr != character.PhotonView.OwnerActorNr)
            {
                PV.TransferOwnership(character.PhotonView.OwnerActorNr);
            }
        }
        _holdCharacter = character;
        // 각 아이템이 잡혔을 때 해줄 초기화 로직
        // 찾은 음식을 플레이어의 손 위치로 이동시킴
        transform.parent = handTransform;
        transform.localPosition = Vector3.zero;
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
        if (place  == null)
        {
            return;
        }
        transform.position = place.position;
        transform.rotation = place.rotation;
        //placeRotation = Quaternion.Euler(0, 0, 0);
        transform.parent = place;
        _holdCharacter = null;
    }
}
