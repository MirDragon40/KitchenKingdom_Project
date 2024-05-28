using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DishObject : IHoldable
{
    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);
   

    public override void Hold(Character character, Transform handTransform)
    {
        transform.parent = handTransform;
        transform.localPosition = new Vector3(0, 0.4f, 0.5F);
        transform.localRotation = Quaternion.identity;
    }

    public override void Place(Vector3 placePosition, Quaternion placeRotation)
    {
        throw new System.NotImplementedException();
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
}
