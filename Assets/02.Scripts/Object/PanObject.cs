using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class PanObject : IHoldable
{
    public Transform PanPlacePositon;
    private Rigidbody _rigid;
    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);

    public override void Hold(Character character, Transform handTransform)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = handTransform;
        transform.localPosition = new Vector3(0, 0.4f, 0.8F);
        transform.localRotation = Quaternion.Euler(-90f, 180f, 0f);
    }

    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        // 저장한 위치와 회전으로 음식 배치
        transform.position = dropPosition;
        //transform.rotation = dropRotation;
        Quaternion pandropRotation = Quaternion.Euler(-90f, 180f, 0f);
        Quaternion finalRotation = dropRotation * pandropRotation;


        transform.parent = null;
        //각 아이템이 떼어질 때 해줄 초기화 로직
        _holdCharacter = null;
    }

    public override void Place(Transform place)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = place.position;
            Quaternion panplaceRotation = Quaternion.Euler(-90, 0, 180);
            transform.rotation = place.rotation * panplaceRotation;
            transform.parent = place;

            //transform.rotation = placeRotation;
            _holdCharacter = null;
        
    }
}




