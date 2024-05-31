using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;


public enum FoodState
{
    Raw,
    Cut,
    Grilled,
    Fried,
    Burnt,
}

public class FoodObject : IHoldable, IThrowable
{

    private EItemType itemType = EItemType.Food;
    public FoodState State;
    public FoodType FoodType;


    public GameObject FoodPrefab1;
    public GameObject FoodPrefab2;
    public GameObject FoodPrefab3;

    private Rigidbody _rigidbody;

    [HideInInspector]
    public bool IsCooking = false;
    public bool IsGrillable = false;
    public bool IsFryable = false;
    public float CookProgress;

    private Coroutine cookingCoroutine;

    private BoxCollider colliderThis;

    public float CuttingTime = 3f;
    public float BakeTime = 3f;


    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);
    //public override Quaternion DropOffset_Rotation => Quaternion.Euler(0, 0, 0);

    public override bool IsProcessed => false;


    private void Awake()
    {

        State = FoodState.Raw;
        _rigidbody = GetComponent<Rigidbody>();
        colliderThis = GetComponent<BoxCollider>();

        CookProgress = 0f;

        if (FoodType == FoodType.Lettuce)
        {
            FoodPrefab1.SetActive(true);
            FoodPrefab2.SetActive(false);
            FoodPrefab3.SetActive(false);
        }
        if (FoodType == FoodType.Patty)
        {
            IsGrillable = true;
            FoodPrefab1.SetActive(true);
            FoodPrefab2.SetActive(false);
            FoodPrefab3.SetActive(false);
        }
        if(FoodType == FoodType.Potato)
        {
            IsFryable = true;
            FoodPrefab1.SetActive(true);
            FoodPrefab2.SetActive(false);
            FoodPrefab3.SetActive(false);
        }
        /*        if (FoodType == FoodType.Patty && State == FoodState.Raw)
                {
                    IsGrillable = true;
                }*/


    }

    private void Update()
    {
        if (IsCooking && cookingCoroutine == null)
        {
            if (FoodType == FoodType.Lettuce)
            {
                cookingCoroutine = StartCoroutine(CookLettuce_Coroutine());
            }
            else if (FoodType == FoodType.Patty)
            {
                cookingCoroutine = StartCoroutine(CookPatty_Coroutine());
            }
            else if (FoodType == FoodType.Potato)
            {
                cookingCoroutine = StartCoroutine(CookPotato_Coroutine());
            }
        }
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

    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void ThrowObject(Vector3 direction, float throwPower)
    {
        _rigidbody.isKinematic = false;
        transform.parent = null;
        _holdCharacter = null;
        
        _rigidbody.AddForce(direction*throwPower, ForceMode.Impulse);
    }

    public override void Place(Transform place)
    {
        transform.position = place.position;
        _rigidbody.isKinematic = true;
        //transform.rotation = placeRotation;
        transform.parent = place;
        _holdCharacter = null;
    }

    private IEnumerator CookLettuce_Coroutine()
    {
        while (IsCooking && State == FoodState.Raw)  // 잘리지 않은 상태에서의 양배추만 썰기가 가능하도록 설정
        {
            colliderThis.enabled = false;

            CookProgress += Time.deltaTime / CuttingTime; // 3초동안 CookProgress 증가
            CookProgress = Mathf.Clamp(CookProgress, 0f, 1f);

            if (CookProgress >= 0.6f && FoodPrefab1.activeSelf)
            {
                FoodPrefab1.SetActive(false);
                FoodPrefab2.SetActive(true);
            }
            if (CookProgress >= 0.9f && FoodPrefab2.activeSelf)
            {
                FoodPrefab2.SetActive(false);
                FoodPrefab3.SetActive(true);
            }
            if (CookProgress >= 1f)
            {
                State = FoodState.Cut;
            }
            yield return null;
        }

        cookingCoroutine = null;
        colliderThis.enabled = true;
    }
    private IEnumerator CookPatty_Coroutine()
    {
        while (IsCooking && (State == FoodState.Raw || State == FoodState.Grilled || State == FoodState.Burnt))
        {
            colliderThis.enabled = false;

            CookProgress += Time.deltaTime / BakeTime;
            CookProgress = Mathf.Clamp(CookProgress, 0f, 2.5f);

            if (State == FoodState.Raw && CookProgress >= 1f && FoodPrefab1.activeSelf)
            {
                FoodPrefab1.SetActive(false);
                FoodPrefab2.SetActive(true);
                State = FoodState.Grilled; // 수정: 상태를 Grilled로 변경
            }
            if (State == FoodState.Grilled && CookProgress >= 2.5f && FoodPrefab2.activeSelf)
            {
                FoodPrefab2.SetActive(false);
                FoodPrefab3.SetActive(true);
                State = FoodState.Burnt; // 수정: 상태를 Burnt로 변경
            }
            yield return null;
        }
        cookingCoroutine = null;
        colliderThis.enabled = true;
    }

    private IEnumerator CookPotato_Coroutine()
    {
        while (IsCooking && (State == FoodState.Raw || State == FoodState.Fried || State == FoodState.Burnt))
        {
            colliderThis.enabled = false;

            CookProgress += Time.deltaTime / BakeTime;
            CookProgress = Mathf.Clamp(CookProgress, 0f, 2.5f);

            if (State == FoodState.Raw && CookProgress >= 1f && FoodPrefab1.activeSelf)
            {
                FoodPrefab1.SetActive(false);
                FoodPrefab2.SetActive(true);
                State = FoodState.Fried; // 수정: 상태를 F로 변경
            }
            if (State == FoodState.Fried && CookProgress >= 2.5f && FoodPrefab2.activeSelf)
            {
                FoodPrefab2.SetActive(false);
                FoodPrefab3.SetActive(true);
                State = FoodState.Burnt; // 수정: 상태를 Burnt로 변경
            }
            yield return null;
        }
        cookingCoroutine = null;
        colliderThis.enabled = true;
    }


    public void StartCooking()
    {
        IsCooking = true;
    }

    public void StopCooking()
    {
        IsCooking = false;
    }
}
