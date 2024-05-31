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

    public GameObject PlusImage;

    private Rigidbody _rigid;

    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();

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
                    FryingIngrediant.StartGrilling(); // Start cooking when placed on the stove
                    FryingSlider.value = FryingIngrediant.CookProgress;
                }
            }
            else
            {
                FryingIngrediant.StopGrilling();
            }

        }
        else
        {
            FryingIngrediant = null;

            FryingSlider.gameObject.SetActive(false);
            PlusImage.SetActive(true);
        }
    }
    public override void Hold(Character character, Transform handTransform)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = handTransform;
        transform.localPosition = new Vector3(0, 0.4f, 0.413f);
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
        transform.position = place.position + new Vector3(0, 0.4f, 0.7f);
        Quaternion basketplaceRotation = Quaternion.Euler(0, 180, 0);
        transform.rotation = place.rotation * basketplaceRotation;
        transform.parent = place;

        MyFryMachine = place.GetComponentInParent<FryMachine>();
        //transform.rotation = placeRotation;
        _holdCharacter = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        IHoldable playerHoldingItem = other.GetComponent<CharacterHoldAbility>().HoldableItem;

        if (playerHoldingItem.GetComponent<FoodObject>().IsGrillable)
        {
            Debug.Log("grill");
            other.GetComponent<CharacterHoldAbility>().PlacePosition = BasketPlacePositon;
            other.GetComponent<CharacterHoldAbility>().IsPlaceable = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<CharacterHoldAbility>().PlacePosition = null;
        other.GetComponent<CharacterHoldAbility>().IsPlaceable = false;
    }
}

