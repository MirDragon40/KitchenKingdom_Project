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

    private Rigidbody _rigid;

    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);

    private void Update()
    {
        if (MyFryMachine != null)
        {
            MyFryMachine = GetComponentInParent<FryMachine>();
        }
        if (BasketPlacePositon.childCount != 0)
        {
            if (BasketPlacePositon.GetChild(0).TryGetComponent<FoodObject>(out FryingIngrediant))
            {
                FryingSlider.gameObject.SetActive(true);

                FryingIngrediant.CookProgress += Time.deltaTime / FryingTime;
                FryingSlider.value = FryingIngrediant.CookProgress;
            }
        }
        else
        {
            FryingIngrediant = null;
            FryingSlider.gameObject.SetActive(false);
        }
    }
    public override void Hold(Character character, Transform handTransform)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = handTransform;
        transform.localPosition = new Vector3(0.005f, 0.4f, 0.363f);
        transform.localRotation = Quaternion.Euler(0, 180f, 0f);
    }

    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        // 저장한 위치와 회전으로 음식 배치
        transform.position = dropPosition + new Vector3(0,0.4f,0f);
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
        Quaternion panplaceRotation = Quaternion.Euler(0, 180f, 0);
        transform.rotation = place.rotation * panplaceRotation;
        transform.parent = place;

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

        if (playerHoldingItem.GetComponent<FoodObject>().IsFryable)
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

