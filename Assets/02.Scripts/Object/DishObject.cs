using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DishState
{
    Dirty,
    Clean
}

public class DishObject : IHoldable
{
    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);

    public DishState State;



    public override void Hold(Character character, Transform handTransform)
    {

        Debug.Log("플레이어가 접시를 들고있다.");
        transform.parent = handTransform;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = false;
        transform.localPosition = new Vector3(0, 0.4f, 0.5F);
        transform.localRotation = Quaternion.identity;
    }

    public override void Place(Transform place)
    {

        transform.position = place.position;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = true;
        transform.rotation = place.rotation;
        //placeRotation = Quaternion.Euler(0, 0, 0);
        transform.parent = place;
        _holdCharacter = null;


    }

    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<BoxCollider>().enabled = false;
        // 저장한 위치와 회전으로 음식 배치
        transform.position = dropPosition;
        transform.rotation = dropRotation;


        transform.parent = null;
        //각 아이템이 떼어질 때 해줄 초기화 로직
        _holdCharacter = null;


    }
}
