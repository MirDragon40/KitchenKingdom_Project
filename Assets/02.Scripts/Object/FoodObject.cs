using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


public enum FoodState
{
    Raw,
    Cut,
}



public class FoodObject : IHoldable, IThrowable
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
    public void ThrowObject(Vector3 direction)
    {
        _rigidbody.isKinematic = false;
        transform.parent = null;
        _holdCharacter = null;
        
        _rigidbody.AddForce(direction*10f, ForceMode.Impulse);
    }

    public override void Place(Transform place)
    {
        transform.position = place.position;
        //transform.rotation = placeRotation;
        transform.parent = place;
        _holdCharacter = null;
    }
}
