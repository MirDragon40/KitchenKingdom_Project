using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PanObject : IHoldable
{
    public Transform PanPlacePositon;
    public FoodObject GrillingIngrediant;
    public float GrillingTime = 5.0f;
    public Slider GrillingSlider;
    public BoxCollider BoxCollider;
    public Stove MyStove;

    public GameObject PlusImage;

    private Rigidbody _rigid;
    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);

    private bool isOnSurface = false;  // 표면 위에 있는지 여부를 추적

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (PanPlacePositon.childCount != 0)
        {
            PlusImage.SetActive(false);

            if (MyStove != null)
            {
                if (PanPlacePositon.GetChild(0).TryGetComponent<FoodObject>(out GrillingIngrediant))
                {
                    GrillingSlider.gameObject.SetActive(true);
                    GrillingIngrediant.StartGrilling(); // Start cooking when placed on the stove
                    GrillingSlider.value = GrillingIngrediant.CookProgress;
                }
            }
            else
            {
                if (GrillingIngrediant != null)
                {
                    GrillingIngrediant.StopGrilling();
                }
            }
        }
        else
        {
            if (GrillingIngrediant != null)
            {
                GrillingIngrediant.StopGrilling();
            }

            GrillingIngrediant = null;

            GrillingSlider.gameObject.SetActive(false);
            PlusImage.SetActive(true);
        }
    }

    public override void Hold(Character character, Transform handTransform)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = handTransform;
        transform.localPosition = new Vector3(0, 0.4f, 0.8F);
        transform.localRotation = Quaternion.Euler(-90f, 180f, 0f);

        MyStove = null;
        isOnSurface = false;  // 아이템을 들 때는 표면에 있지 않음
    }

    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {
        if (isOnSurface)  // 표면 위에 있을 때만 UnHold 동작
        {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.position = dropPosition;
            Quaternion pandropRotation = Quaternion.Euler(-90f, 180f, 0f);
            Quaternion finalRotation = dropRotation * pandropRotation;

            transform.parent = null;
            _holdCharacter = null;
        }
    }

    public override void Place(Transform place)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = place.position;
        Quaternion panplaceRotation = Quaternion.Euler(-90, 0, 180);
        transform.rotation = place.rotation * panplaceRotation;
        transform.parent = place;

        MyStove = place.GetComponentInParent<Stove>();
        _holdCharacter = null;
        isOnSurface = true;  // 아이템을 놓을 때 표면 위에 있음
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table"))  // 표면에 닿았을 때
        {
            isOnSurface = true;
        }
        else if (other.CompareTag("Player"))
        {
            IHoldable playerHoldingItem = other.GetComponent<CharacterHoldAbility>().HoldableItem;

            if (playerHoldingItem != null && playerHoldingItem.GetComponent<FoodObject>().IsGrillable)
            {
                Debug.Log("grill");
                other.GetComponent<CharacterHoldAbility>().PlacePosition = PanPlacePositon;
                other.GetComponent<CharacterHoldAbility>().IsPlaceable = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table"))  // 표면에서 벗어났을 때
        {
            isOnSurface = false;
        }
        else
        {
            CharacterHoldAbility holdAbility = other.GetComponent<CharacterHoldAbility>();
            if (holdAbility != null)
            {
                holdAbility.PlacePosition = null;
                holdAbility.IsPlaceable = false;
            }
        }
    }
}