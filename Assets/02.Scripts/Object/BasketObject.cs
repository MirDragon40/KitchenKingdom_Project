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

    public FireObject fireObject;
    public DangerIndicator dangerIndicator;
    public Sprite dangerSprite;
    public Slider FireSlider;
    private SoundManager soundManager;
    private bool isPowderTouching = false;

    private Rigidbody _rigid;

    private bool hasCaughtFireOnce = false;
    private bool hasDangerIndicator = false;
    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);
    private bool isAnyBasketOnFire = false;

    public Table[] NearbyTables;

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        _rigid = GetComponent<Rigidbody>();
        _pv = GetComponent<PhotonView>();
        dangerIndicator = GetComponentInChildren<DangerIndicator>();
        FireSlider.gameObject.SetActive(false);
        fireObject = GetComponent<FireObject>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void Start()
    {
        if (BasketStartPosition != null)
        {
            Place(BasketStartPosition);
        }
        if (FireSlider != null)
        {
            FireSlider.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (BasketPlacePositon.childCount != 0)
        {
            PlusImage.SetActive(false);

            if (MyFryMachine != null)
            {
                if (BasketPlacePositon.GetChild(0).TryGetComponent<FoodObject>(out FoodObject newGrillingIngredient))
                {
                    if (FryingIngrediant != newGrillingIngredient)
                    {
                        FryingIngrediant = newGrillingIngredient;
                        hasCaughtFireOnce = false;
                    }

                    FryingIngrediant.StartFrying();
                    FryingSlider.gameObject.SetActive(true);
                    FryingSlider.value = FryingIngrediant.CookProgress;

                    // 위험 표시기 표시
                    if (FryingIngrediant.CookProgress >= 2f && FryingIngrediant.CookProgress < 4.9f)
                    {
                        dangerIndicator.ShowDangerIndicator(dangerSprite);
                        soundManager.PlayAudio("Warning", true, true);
                        hasDangerIndicator = true;
                    }
                    else
                    {
                        dangerIndicator.HideDangerIndicator();
                        hasDangerIndicator = false;
                    }

                    // 불이 켜지는 시점
                    if (FryingIngrediant.CookProgress >= 5f && !fireObject._isOnFire && !hasCaughtFireOnce)
                    {
                        fireObject.MakeFire();
                        FireSlider.gameObject.SetActive(true);
                        hasCaughtFireOnce = true;
                        Debug.Log("켜짐");
                    }
                }

                if (fireObject._isOnFire && !MyFryMachine.fireObject._isOnFire)
                {
                    MyFryMachine.fireObject.MakeFire();
                }
                else if (!fireObject._isOnFire && MyFryMachine.fireObject._isOnFire)
                {
                    MyFryMachine.fireObject.RequestExtinguish();
                }
            }
            else
            {
                if (FryingIngrediant != null)
                {
                    FryingIngrediant.StopFrying();
                }
            }

            FryingSlider.gameObject.SetActive(FryingSlider.value < 1f);
        }
        else
        {
            PlusImage.SetActive(true);

            if (FryingIngrediant != null)
            {
                FryingIngrediant.StopFrying();
            }

            FryingIngrediant = null;
            FryingSlider.gameObject.SetActive(false);
        }

        // 분말과의 불 접촉 시간 관리
        if (!isPowderTouching && fireObject.contactTime > 0)
        {
            fireObject.contactTime -= Time.deltaTime;
            if (fireObject.contactTime < 0)
            {
                fireObject.contactTime = 0;
            }
        }

        isPowderTouching = false; // 매 프레임마다 false로 초기화

        // 불 슬라이더 표시 관리
        if (fireObject._isOnFire && FireSlider != null)
        {
            FireSlider.value = fireObject.contactTime / 2f;
        }
        else if (!fireObject._isOnFire && FireSlider.gameObject.activeSelf)
        {
            FireSlider.gameObject.SetActive(false);
        }

        if (_isNearTrashBin && Input.GetKeyDown(KeyCode.Space))
        {
            DropFoodInTrash();
        }
    }

    public override void Hold(Character character, Transform handTransform)
    {
        if (character == null || handTransform == null)
        {
            return;
        }
        int charOwnerActorNr = character.PhotonView.OwnerActorNr;
        if (_pv.OwnerActorNr != charOwnerActorNr)
        {
            _pv.TransferOwnership(charOwnerActorNr);
            if (FryingIngrediant != null)
            {
                FryingIngrediant.GetComponent<PhotonView>().TransferOwnership(charOwnerActorNr);
            }
        }
        _holdCharacter = character;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(handTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 180f, 0f);

        MyFryMachine = null;

        dangerIndicator.HideDangerIndicator();
        soundManager.StopAudio("Warning");
    }

    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        transform.position = dropPosition + new Vector3(0, 0.6f, 0f);
        Quaternion pandropRotation = Quaternion.Euler(0, 0, 0f);
        Quaternion finalRotation = dropRotation * pandropRotation;

        transform.parent = null;
        _holdCharacter = null;
    }

    public override void Place(Transform place)
    {
        if (place == null)
        {
            return;
        }
        GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(place);
        transform.localPosition = Vector3.zero;

        var pivotOffset = new Vector3(0f, 0.5f, -0.6f);
        Vector3 pivotPosition = transform.localPosition + pivotOffset;
        transform.localPosition = pivotPosition;
        transform.localRotation = Quaternion.identity;
        transform.Rotate(new Vector3(0, 180, 0));

        if (MyFryMachine != null)
        {
            MyFryMachine.PlacedBasket = this;
        }

        if (MyFryMachine != null)
        {
            MyFryMachine.PlacedBasket = this;

            if (fireObject._isOnFire)
            {
                MyFryMachine.fireObject.RequestMakeFire();
            }
            else
            {
                MyFryMachine.fireObject.RequestExtinguish();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        IHoldable playerHoldingItem = other.GetComponent<CharacterHoldAbility>().HoldableItem;
        FoodObject foodObject = null;
        if (playerHoldingItem != null)
        {
            if (playerHoldingItem.TryGetComponent<FoodObject>(out foodObject))
            {
                if (foodObject.IsFryable)
                {
                    other.GetComponent<CharacterHoldAbility>().PlacePosition = BasketPlacePositon;
                    other.GetComponent<CharacterHoldAbility>().IsPlaceable = true;
                }
            }
        }
        if (fireObject._isOnFire && other.CompareTag("Powder"))
        {
            isPowderTouching = true;
            fireObject.contactTime += Time.deltaTime;
            Debug.Log(fireObject.contactTime);
            if (fireObject.contactTime >= 1f)
            {
                fireObject.RequestExtinguish();
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

        if (fireObject._isOnFire && other.CompareTag("Powder"))
        {
            isPowderTouching = false;
        }
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