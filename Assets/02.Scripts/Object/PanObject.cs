using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class PanObject : IHoldable
{
    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);

    public override void Hold(Character character, Transform handTransform)
    {
        transform.parent = handTransform;
        transform.localPosition = new Vector3(0, 0.4f, 0.8F);
        transform.localRotation = Quaternion.Euler(-90f, 180f, 0f);
    }

    public override void Place(Vector3 placePosition, Quaternion placeRotation)
    {
        {
            transform.position = placePosition;
            //transform.rotation = placeRotation;
            Quaternion panplaceRotation = Quaternion.Euler(-90,0,180);
            transform.parent = null;
            _holdCharacter = null;
        }
    }

    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {
        // 저장한 위치와 회전으로 음식 배치
        transform.position = dropPosition;
        //transform.rotation = dropRotation;
        Quaternion pandropRotation = Quaternion.Euler(-90f, 180f, 0f);
        Quaternion finalRotation = dropRotation * pandropRotation;


        transform.parent = null;
        //각 아이템이 떼어질 때 해줄 초기화 로직
        _holdCharacter = null;
    }
}




