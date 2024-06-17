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

    private bool isPowderTouching = false;

    internal bool isOnFire;
    private bool isNearTrashBin = false;
    private TrashBin nearbyTrashBin;

    public Transform PanStartPosition; // 팬 초기위치

    private bool hasCaughtFireOnce = false;
    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        fireObject = GetComponent<FireObject>();
        dangerIndicator = GetComponentInChildren<DangerIndicator>();
        FireSlider.gameObject.SetActive(false);
    }
    private void Start()
    {
        // 게임 시작 시 팬을 초기 배치 위치로 이동시킴
        if (PanStartPosition != null)
        {
            Place(PanStartPosition);
        }
        if (FireSlider != null)
        {
            FireSlider.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        // 팬이 스토브에 놓인 경우
        if (PanPlacePositon.childCount != 0)
        {
            PlusImage.SetActive(false);

            // 스토브가 할당되어 있을 경우
            if (MyStove != null)
            {
                // 팬에 음식이 있을 때만 그릴링과 불 동작
                if (PanPlacePositon.GetChild(0).TryGetComponent<FoodObject>(out FoodObject newGrillingIngredient))
                {
                    if (GrillingIngrediant != newGrillingIngredient)
                    {
                        GrillingIngrediant = newGrillingIngredient;
                        hasCaughtFireOnce = false;
                    }

                    GrillingIngrediant.StartGrilling();
                    GrillingSlider.gameObject.SetActive(true);
                    GrillingSlider.value = GrillingIngrediant.CookProgress;

                    // 위험 표시기 표시
                    if (GrillingIngrediant.CookProgress >= 2f && GrillingIngrediant.CookProgress < 2.9f)
                    {
                        dangerIndicator.ShowDangerIndicator(dangerSprite);
                    }
                    else
                    {
                        dangerIndicator.HideDangerIndicator();
                    }

                    // 불이 켜지는 시점
                    if (GrillingIngrediant.CookProgress >= 3f && !fireObject._isOnFire && !hasCaughtFireOnce)
                    {
                        fireObject.MakeFire();
                        FireSlider.gameObject.SetActive(true);
                        hasCaughtFireOnce = true;
                    }
                    else if (GrillingIngrediant.CookProgress < 3f && fireObject._isOnFire)
                    {
                        fireObject.Extinguish();
                        FireSlider.gameObject.SetActive(false);
                    }
                }

                // 스토브의 불 상태 확인 및 조작
                if (fireObject._isOnFire && !MyStove.fireObject._isOnFire)
                {
                    MyStove.fireObject.MakeFire();
                }
                else if (!fireObject._isOnFire && MyStove.fireObject._isOnFire)
                {
                    MyStove.fireObject.Extinguish();
                }
            }
            else
            {
                // 할당된 스토브가 없으면 그릴링 중단
                if (GrillingIngrediant != null)
                {
                    GrillingIngrediant.StopGrilling();
                }
            }

            // 그릴링 슬라이더 표시 관리
            GrillingSlider.gameObject.SetActive(GrillingSlider.value < 1f);
        }
        else // 팬이 스토브에 놓이지 않은 경우
        {
            PlusImage.SetActive(true);

            // 팬이 불타고 있으면 불 끄기
            if (fireObject._isOnFire)
            {
                fireObject.Extinguish();
            }

            // 음식 그릴링 중단
            if (GrillingIngrediant != null)
            {
                GrillingIngrediant.StopGrilling();
            }

            GrillingIngrediant = null;
            GrillingSlider.gameObject.SetActive(false);
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

        // 쓰레기통 근처에서 스페이스바 눌렀을 때 음식 버리기
        if (isNearTrashBin && Input.GetKeyDown(KeyCode.Space))
        {
            DropFoodInTrash();
        }
    }

    public override void Hold(Character character, Transform handTransform)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = handTransform;
        transform.localPosition = new Vector3(0, 0, 0.3f);
        transform.localRotation = Quaternion.Euler(-90f, 180f, 0f);

        MyStove = null;
        isOnSurface = false;  // 아이템을 들 때는 표면에 있지 않음

        dangerIndicator.HideDangerIndicator();
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
        if (place == null)
        {
            return;
        }
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = place.position;
        Quaternion panplaceRotation = Quaternion.Euler(-90, 0, 180);
        transform.rotation = place.rotation * panplaceRotation;
        transform.parent = place;
        Stove stoveInParent = place.GetComponentInParent<Stove>();
        if (stoveInParent != null)
        {
            MyStove = stoveInParent;
        }
        if (_holdCharacter != null)
        {
            _holdCharacter = null;
        }
      
        isOnSurface = true;  // 아이템을 놓을 때 표면 위에 있음

        if (MyStove != null)
        {
            MyStove.PlacedPan = this;

            if (fireObject._isOnFire)
            {
                MyStove.fireObject.MakeFire();
            }
            else
            {
                MyStove.fireObject.Extinguish();
            }
        }
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
            FoodObject food = null;
            if (playerHoldingItem != null)
            {
                if (playerHoldingItem.TryGetComponent<FoodObject>(out food))
                {
                    if (food.IsGrillable)
                    {
                        other.GetComponent<CharacterHoldAbility>().PlacePosition = PanPlacePositon;
                        other.GetComponent<CharacterHoldAbility>().IsPlaceable = true;
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (fireObject._isOnFire && other.CompareTag("Powder"))
        {
            isPowderTouching = true;
            fireObject.contactTime += Time.deltaTime;
            Debug.Log(fireObject.contactTime);
            if (fireObject.contactTime >= 2f)
            {
                fireObject.Extinguish();
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
        if (fireObject._isOnFire && other.CompareTag("Powder"))
        {
            isPowderTouching = false;
        }
    }


    public void SetNearTrashBin(bool isNear, TrashBin trashBin = null)
    {
        isNearTrashBin = isNear;
        nearbyTrashBin = trashBin;
    }

    private void DropFoodInTrash()
    {
        if (PanPlacePositon.childCount > 0)
        {
            for (int i = PanPlacePositon.childCount - 1; i >= 0; i--)
            {
                Transform child = PanPlacePositon.GetChild(i);
                FoodObject childFoodObject = child.GetComponent<FoodObject>();
                if (childFoodObject != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}