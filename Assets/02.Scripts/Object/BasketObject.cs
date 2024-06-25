
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasketObject : IHoldable
{

    public Transform BasketPlacePositon;
    public FoodObject FryingIngrediant;
    public float FryingTime = 5.0f;
    public Slider FryingSlider;
    public BoxCollider BoxCollider;
    public FryMachine MyFryMachine;
    public Transform BasketStartPosition;
    private PhotonView _pv;

    private bool _isNearTrashBin = false;
    private TrashBin _nearbyTrashBin;
    public GameObject PlusImage;

    private Rigidbody _rigid;

    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        _rigid = GetComponent<Rigidbody>();
        _pv = GetComponent<PhotonView>();

    }
    private void Start()
    {
        if (BasketStartPosition != null )
        {
            Place(BasketStartPosition);
        }

    }
    private void Update()
    {
        if (BasketPlacePositon.childCount != 0)
        {
            PlusImage.SetActive(false);

                if (MyFryMachine != null)
                {
                    if (BasketPlacePositon.GetChild(0).TryGetComponent<FoodObject>(out FryingIngrediant))
                    {
                        FryingSlider.gameObject.SetActive(true);
                        FryingIngrediant.StartFrying(); // Start cooking when placed on the stove
                        FryingSlider.value = FryingIngrediant.CookProgress;
                    }
                }
                else if (FryingIngrediant != null)
                {
                    FryingIngrediant.StopFrying();
                }
            
        }
        else
        {
            FryingIngrediant = null;

            FryingSlider.gameObject.SetActive(false);
            PlusImage.SetActive(true);
        }
        if (_isNearTrashBin && Input.GetKeyDown(KeyCode.Space))
        {
            DropFoodInTrash();
        }
    }
    public override void Hold(Character character, Transform handTransform)
    {
        _holdCharacter = character;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(handTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 180f, 0f);

        MyFryMachine = null;
    }

    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        // 저장한 위치와 회전으로 음식 배치
        transform.position = dropPosition + new Vector3(0, 0.6f, 0f);
        //transform.rotation = dropRotation;
        Quaternion pandropRotation = Quaternion.Euler(0, 0, 0f);
        Quaternion finalRotation = dropRotation * pandropRotation;


        transform.parent = null;
        //각 아이템이 떼어질 때 해줄 초기화 로직
        _holdCharacter = null;
    }

    public override void Place(Transform place)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(place);
        transform.localPosition = Vector3.zero;

/*        if (_holdCharacter != null)
        {*/
            // 물체 중간 오프셋
            var pivotOffset = new Vector3(0f, 0.5f, -0.6f);

            // 현재 위치를 기준으로 중간 위치 계산
            Vector3 pivotPosition = transform.localPosition + pivotOffset;

            // 물체를 중간 위치로 이동
            transform.localPosition = pivotPosition;
            //var targetRotation = _holdCharacter.transform.rotation;
            // 플레이어가 쳐다보는 방향으로 회전 적용
            // 플레이어가 쳐다보는 방향으로 회전 각도 계산
            transform.localRotation = Quaternion.identity;
            //transform.Rotate(new Vector3(0, 180 + targetRotation.eulerAngles.y, 0));
            transform.Rotate(new Vector3(0, 180, 0));
/*
        }*/

        // 선반 오프셋 적용
        //transform.localPosition += new Vector3(0.3f, 0.4f, 0.7f);

        //transform.position = place.position + GetPositionAfterRotation(new Vector3(0, 0.4f, 0.5f), targetRotation.eulerAngles.y, 1);


        //Quaternion basketplaceRotation = Quaternion.Euler(0, 180, 0);
        //transform.rotation = place.rotation * basketplaceRotation;




        if (MyFryMachine != null)
        {
            MyFryMachine.PlacedBasket = this;
        }

        _holdCharacter = null;
    }

    Vector3 GetPositionAfterRotation(Vector3 startPosition, float angle, float distance)
    {
        // 각도를 라디안으로 변환
        float angleInRadians = angle * Mathf.Deg2Rad;

        // 새로운 위치 계산
        float newX = startPosition.x + distance * Mathf.Cos(angleInRadians);
        float newZ = startPosition.z + distance * Mathf.Sin(angleInRadians);

        // y값은 변화가 없다고 가정
        return new Vector3(newX, startPosition.y, newZ);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        int charOwnerActorNr = other.GetComponent<Character>().PhotonView.OwnerActorNr;
        if (_pv.OwnerActorNr != charOwnerActorNr)
        {
            _pv.TransferOwnership(charOwnerActorNr);
        }
        IHoldable playerHoldingItem = other.GetComponent<CharacterHoldAbility>().HoldableItem;
        FoodObject foodObject = null;
        if (playerHoldingItem != null)
        {
            if (playerHoldingItem.TryGetComponent<FoodObject>(out foodObject))
            {
                if (foodObject.IsFryable)
                {
                   // Debug.Log("Fry");
                    other.GetComponent<CharacterHoldAbility>().PlacePosition = BasketPlacePositon;
                    other.GetComponent<CharacterHoldAbility>().IsPlaceable = true;
                }
            }

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        other.GetComponent<CharacterHoldAbility>().PlacePosition = null;
        other.GetComponent<CharacterHoldAbility>().IsPlaceable = false;
    }
    public void SetNearTrashBin(bool isNear, TrashBin trashBin = null)
    {
        _isNearTrashBin = isNear;
        _nearbyTrashBin = trashBin;
    }

    private void DropFoodInTrash()
    {
        if (BasketPlacePositon.childCount > 0)
        {
            for (int i = BasketPlacePositon.childCount - 1; i >= 0; i--)
            {
                Transform child = BasketPlacePositon.GetChild(i);
                FoodObject childFoodObject = child.GetComponent<FoodObject>();
                if (childFoodObject != null && _pv.IsMine)
                {
                    PhotonNetwork.Destroy(child.gameObject);
                }
            }
        }
    }
}

