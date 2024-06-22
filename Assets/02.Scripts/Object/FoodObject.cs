using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public SoundManager soundManager;

    public GameObject FoodPrefab1;
    public GameObject FoodPrefab2;
    public GameObject FoodPrefab3;


    public PhotonView PV;

    private Rigidbody _rigidbody;

    [HideInInspector]
    public bool IsCooking = false;
    public bool IsGrillable = false;
    public bool IsFryable = false;
    public float CookProgress;

    private Coroutine cookingCoroutine;

    private BoxCollider colliderThis;
    public float ThrownEffectTriggerTime = 0.5f;
    public float CuttingTime = 3f;
    public float BakeTime = 3f;

    private static int grillingCount = 0;

    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);
    //public override Quaternion DropOffset_Rotation => Quaternion.Euler(0, 0, 0);

    public override bool IsProcessed => false;



    private void Awake()
    {

        PV = GetComponent<PhotonView>();
        State = FoodState.Raw;
        _rigidbody = GetComponent<Rigidbody>();
        colliderThis = GetComponent<BoxCollider>();
        soundManager = FindAnyObjectByType<SoundManager>();

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
        if (FoodType == FoodType.Potato)
        {
            IsFryable = true;
            FoodPrefab1.SetActive(true);
            FoodPrefab2.SetActive(false);
            FoodPrefab3.SetActive(false);
        }
        
    }

    public float DAMPING = 1f;

    private void Update()
    {
        if(_holdCharacter != null)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

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
        if(PhotonNetwork.IsMasterClient)
        {
            if (PV.OwnerActorNr != character.PhotonView.OwnerActorNr)
            {
                PV.TransferOwnership(character.PhotonView.OwnerActorNr);
            }
        }

        _rigidbody.isKinematic = true;
        _holdCharacter = character;
        _holdCharacter.HoldAbility.HoldableItem = this;
        _holdCharacter.HoldAbility.JustHold = true;

        // 각 아이템이 잡혔을 때 해줄 초기화 로직
        // 찾은 음식을 플레이어의 손 위치로 이동시킴
        transform.parent = handTransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }


    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {
        if (_holdCharacter.GetComponent<CharacterHoldAbility>().IsPlaceable == true)
        {
            return;
        }
        _rigidbody.isKinematic = false;
        // 저장한 위치와 회전으로 음식 배치
        /*        transform.position = dropPosition;
                transform.rotation = dropRotation;*/
        

        transform.SetParent(null);
        //각 아이템이 떼어질 때 해줄 초기화 로직
        _holdCharacter = null;


    }

    public new void Destroy()
    {
        if (PV.IsMine)
        {

            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void ThrowObject(Vector3 direction, float throwPower)
    {
        _rigidbody.isKinematic = false;
        transform.SetParent(null);
        _holdCharacter = null;
        _rigidbody.AddForce(direction * throwPower, ForceMode.Impulse);
        StartCoroutine(TriggerThrownItem_Coroutine());
    }
    private IEnumerator TriggerThrownItem_Coroutine()
    {
        Collider[] colliders = new Collider[4];
        yield return new WaitForSeconds(0.2f);

        CookStand cookStand = null;
        PanObject panObject = null;

        while (true)
        {
            Vector3 speed = _rigidbody.velocity;
            speed.y = 0;

            if (speed.magnitude < 0.9f)
            {

                int colliderNum = Physics.OverlapSphereNonAlloc(transform.position, 0.4f, colliders);
               // Debug.Log(colliderNum);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent<PanObject>(out panObject))
                    {
                        Debug.Log("Pan Found");
                        transform.rotation = Quaternion.identity;
                        if (panObject.GrillingIngrediant == null)
                        {
                            Place(panObject.PanPlacePosition);
                        }
                        colliders = null;
                        break;
                    }
                    else if (collider.TryGetComponent<CookStand>(out cookStand))
                    {
                        transform.rotation = Quaternion.identity;
                        if (!cookStand.IsOccupied)
                        {
                            Place(cookStand.PlacePosition);
                        }
                        colliders = null;
                        break;
                    }

                }
            }
            if (speed.magnitude < 0.1f)
            {
                break;
            }

            yield return new WaitForFixedUpdate();
            if (panObject != null || cookStand != null)
            {
                break;
            }
        }

    }
    public override void Place(Transform place)
    {
        if (place == null)
        {
            return;
        }
        _rigidbody.isKinematic = true;
        transform.position = place.position;
        transform.SetParent(place);
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
            if (CookProgress >= 1f && FoodPrefab2.activeSelf)
            {
                FoodPrefab2.SetActive(false);
                FoodPrefab3.SetActive(true);
                State = FoodState.Cut;
                colliderThis.enabled = true;
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

            CookProgress += Time.fixedDeltaTime / BakeTime;
            CookProgress = Mathf.Clamp(CookProgress, 0f, 5f);
            if (State == FoodState.Raw && CookProgress >= 1f && FoodPrefab1.activeSelf)
            {
                FoodPrefab1.SetActive(false);
                FoodPrefab2.SetActive(true);
                
                State = FoodState.Grilled; // 수정: 상태를 Grilled로 변경
            }

            if (State == FoodState.Grilled && CookProgress >= 5f && FoodPrefab2.activeSelf)
            {
                FoodPrefab2.SetActive(false);
                FoodPrefab3.SetActive(true);
                State = FoodState.Burnt; // 수정: 상태를 Burnt로 변경

            }
            yield return new WaitForFixedUpdate();
        }

        cookingCoroutine = null;
        colliderThis.enabled = true;
    }

    private IEnumerator CookPotato_Coroutine()
    {
        while (IsCooking && (State == FoodState.Raw || State == FoodState.Fried || State == FoodState.Burnt))
        {
            colliderThis.enabled = false;

            CookProgress += Time.fixedDeltaTime / BakeTime;
            CookProgress = Mathf.Clamp(CookProgress, 0f, 2.5f);

            if (State == FoodState.Raw && CookProgress >= 1f && FoodPrefab1.activeSelf)
            {
                FoodPrefab1.SetActive(false);
                FoodPrefab2.SetActive(true);
                State = FoodState.Fried; // 수정: 상태를 Fried로 변경
            }
            if (State == FoodState.Fried && CookProgress >= 2.5f && FoodPrefab2.activeSelf)
            {
                FoodPrefab2.SetActive(false);
                FoodPrefab3.SetActive(true);
                State = FoodState.Burnt; // 수정: 상태를 Burnt로 변경
            }
            yield return new WaitForFixedUpdate();
        }
        cookingCoroutine = null;
        colliderThis.enabled = true;
    }


    public void StartGrilling()
    {
        if (cookingCoroutine == null)
        {
            IsCooking = true;
            cookingCoroutine = StartCoroutine(CookPatty_Coroutine());
            grillingCount++;
            if (grillingCount == 1)
            {
                soundManager.PlayAudio("Patty", true, true);
            }
        }
    }


    public void StopGrilling()
    {
        IsCooking = false;
        if (cookingCoroutine != null)
        {
            StopCoroutine(cookingCoroutine);
            //soundManager.StopAudio("Patty");
            cookingCoroutine = null;
            grillingCount--;
            if (grillingCount == 0)
            {
                soundManager.StopAudio("Patty");
            }
        }
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
