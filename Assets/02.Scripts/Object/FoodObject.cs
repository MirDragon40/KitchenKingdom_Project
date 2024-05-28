using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FoodObject : IHoldable
{

    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);

    public override bool IsProcessed => false;
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

    public void Dispose()
    {
        Destroy(gameObject);
    }

    public override void Place(Vector3 placePosition, Quaternion placeRotation)
    {
        throw new System.NotImplementedException();
    }
}
