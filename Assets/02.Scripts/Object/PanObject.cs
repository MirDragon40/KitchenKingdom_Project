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

    public Slider FireSlider;
    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);

    private bool isOnSurface = false;  // 표면 위에 있는지 여부를 추적

    public FireObject fireObject;
    public DangerIndicator dangerIndicator;
    public Sprite dangerSprite;

    private bool isPowderTouching = false; // 파우더와 닿는지 확인

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        fireObject = GetComponent<FireObject>();
        dangerIndicator = GetComponentInChildren<DangerIndicator>();
        FireSlider.gameObject.SetActive(false);
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
                    if (GrillingIngrediant.CookProgress >= 2f && GrillingIngrediant.CookProgress < 2.9f)
                    {
                        dangerIndicator.ShowDangerIndicator(dangerSprite);
                    }
                    else
                    {
                        dangerIndicator.HideDangerIndicator();
                    }

                    if (GrillingIngrediant.CookProgress >= 3f)
                    {
                        fireObject.MakeFire();
                        FireSlider.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (GrillingIngrediant != null)
                {
                    GrillingIngrediant.StopGrilling();
                }
            }

            if (GrillingSlider.value >= 1f)
            {
                GrillingSlider.gameObject.SetActive(false);
            }
            else
            {
                GrillingSlider.gameObject.SetActive(true);
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

        // 파우더에 닿지 않았을 때 contactTime을 서서히 감소시킴
        if (!isPowderTouching && fireObject.contactTime > 0)
        {
            fireObject.contactTime -= Time.deltaTime;
            Debug.Log(fireObject.contactTime);
            if (fireObject.contactTime < 0)
            {
                fireObject.contactTime = 0;
            }
        }

        isPowderTouching = false;  // 매 프레임마다 false로 초기화

        if (fireObject.isFireActive && FireSlider != null)
        {
            FireSlider.value = fireObject.contactTime / 2f;
        }
        else if (!fireObject.isFireActive && FireSlider.gameObject.activeSelf)
        {
            FireSlider.gameObject.SetActive(false);
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

    private void OnTriggerStay(Collider other)
    {
        // 불이 활성화 상태이고, 'Powder' 태그 오브젝트와 접촉 중일 때
        if (fireObject.isFireActive && other.CompareTag("Powder"))
        {
            isPowderTouching = true;
            fireObject.contactTime += Time.deltaTime; // 접촉 시간을 측정
            Debug.Log(fireObject.contactTime);
            // 접촉 시간이 2초 이상이면 불을 끔
            if (fireObject.contactTime >= 2f)
            {
                fireObject.Extinguish();
                FireSlider.gameObject.SetActive(false);
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
        if (fireObject.isFireActive && other.CompareTag("Powder"))
        {
            isPowderTouching = false;  // 파우더에 더 이상 닿지 않음을 표시
        }
    }
}