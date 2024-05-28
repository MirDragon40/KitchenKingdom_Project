using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


public enum FoodState
{
    Raw,
    Cut,
}



public class FoodObject : IHoldable
{

    private EItemType itemType = EItemType.Food;

    public FoodState State;
    private Rigidbody _rigidbody;

    public FoodType FoodType;


    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);
    //public override Quaternion DropOffset_Rotation => Quaternion.Euler(0, 0, 0);

    public override bool IsProcessed => false;


    private void Awake()
    {
        State = FoodState.Raw;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void Hold(Character character, Transform handTransform)
    {
        _rigidbody.isKinematic = true;
        _holdCharacter = character;

        // 각 아이템이 잡혔을 때 해줄 초기화 로직
        // 찾은 음식을 플레이어의 손 위치로 이동시킴
        transform.parent = handTransform;
        transform.localPosition = new Vector3(0, 0.4F, 0.5F);
        transform.localRotation = Quaternion.identity;
    }



    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {
        _rigidbody.isKinematic = false;
        // 저장한 위치와 회전으로 음식 배치
/*        transform.position = dropPosition;
        transform.rotation = dropRotation;*/


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
        transform.position = placePosition;
        //transform.rotation = placeRotation;
        placeRotation = Quaternion.Euler(0, 0, 0);
        transform.parent = null;
        _holdCharacter = null;
    }
}
