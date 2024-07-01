using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;



public class CharacterHoldAbility : CharacterAbility
{


    public Transform HandTransform;
    private Animator animator;
    private float _findfood = 1f; //음식을 찾는 범위


    public IHoldable HoldableItem;
    // private Transform _placeableSurface;
    public bool IsDroppable => !IsPlaceable && !IsSubmitable && !IsServeable && PlacePosition == null;
    public bool IsPlaceable = false;
    public bool IsSubmitable = false;
    public bool IsServeable = false;
    public bool IsHolding => HoldableItem != null;
    public bool JustHold = false;

    public FoodCombination SelectedDish;

    public PhotonView _pv;
    public Transform PlacePosition = null;

    private bool nearTrashBin = false;
    private Transform _carryTransform; // 플레이어가 들고있는 객체를 참조

    public SoundManager soundManager;
    void Start()
    {
        animator = GetComponent<Animator>();
        _pv = _owner.PhotonView;
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void LateUpdate()
    {
        if (JustHold)
        {
            JustHold = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _owner.PhotonView.IsMine)
        {


            if (!IsHolding)
            {

                _pv.RPC("PickUp", RpcTarget.All);

            }
            else
            {


                if (IsPlaceable)
                {
                    _pv.RPC("Place", RpcTarget.All);
                }

                else if (IsDroppable)
                {
                    if (nearTrashBin)
                    {
                        _pv.RPC("DropFood", RpcTarget.All);
                        soundManager.PlayAudio("Trash", false, true);
                    }
                    else
                    {

                        _pv.RPC("Drop", RpcTarget.All);

                    }
                }
            }
        }
    }

    [PunRPC]
    public void PickUp()
    {
        // 들고 있는 음식이 있으면 아무 작업도 수행하지 않음
        if (IsHolding)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, _findfood);
        IHoldable holdable = null;
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IHoldable>(out holdable))
            {
                if (holdable.IsHold)
                {
                    continue;
                }
                // 팬인 경우
                if (holdable is PanObject pan)
                {
                    // 스토브와 테이블 불 상태 체크
                    Stove stove = pan.GetComponentInParent<Stove>();
                    if (stove != null && stove.IsOnFire)
                    {
                        return; // 스토브가 불이 났으면 팬을 잡지 못하도록 반환
                    }

                    Table[] nearbyTables = pan.NearbyTables;
                    foreach (Table table in nearbyTables)
                    {
                        if (table != null && table.IsOnFire)
                        {
                            return; // 주변 테이블이 불이 났으면 팬을 잡지 못하도록 반환
                        }
                    }

                    // 팬을 잡을 수 있음
                    HoldableItem = holdable;
                    holdable.Hold(_owner, HandTransform);
                    animator.SetBool("Carry", true);
                    return;
                }
                // 바스켓인 경우
                else if (holdable is BasketObject basket)
                {
                    // 프라이머신과 테이블 불 상태 체크
                    FryMachine fryMachine = basket.GetComponentInParent<FryMachine>();
                    if (fryMachine != null && fryMachine.IsOnFire)
                    {
                        return; // 프라이머신이 불이 났으면 바스켓을 잡지 못하도록 반환
                    }

                    Table[] nearbyTables = basket.NearbyTables;
                    foreach (Table table in nearbyTables)
                    {
                        if (table != null && table.IsOnFire)
                        {
                            return; // 주변 테이블이 불이 났으면 바스켓을 잡지 못하도록 반환
                        }
                    }

                    // 바스켓을 잡을 수 있음
                    HoldableItem = holdable;
                    holdable.Hold(_owner, HandTransform);
                    animator.SetBool("Carry", true);
                    return;
                }
            }
        }

        // 스토브나 테이블 위에 있는 것들도 확인
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IHoldable>(out holdable))
            {
                // 오브젝트가 스토브나 테이블과 연결된 경우 불이 난 상태를 확인
                Stove stove = holdable.GetComponentInParent<Stove>();
                Table table = holdable.GetComponentInParent<Table>();
                if ((stove != null && stove.IsOnFire) || (table != null && table.IsOnFire))
                {
                    continue;
                }

                // 팬이나 바스켓에 음식이 있는지 다시 확인
                if (holdable is PanObject pan && pan.GetComponentInChildren<FoodObject>() != null)
                {
                    continue;
                }
                else if (holdable is BasketObject basket && basket.GetComponentInChildren<FoodObject>() != null)
                {
                    continue;
                }

                // 음식이 없는 경우 잡을 수 있음
                HoldableItem = holdable;
                holdable.Hold(_owner, HandTransform);
                animator.SetBool("Carry", true);
                break;
            }

        }
    }
    [PunRPC]
    void Drop()
    {
        // 들고 있는 음식이 없으면 아무 작업도 수행하지 않음
        if (!IsHolding || HoldableItem is PanObject || HoldableItem is BasketObject)
        {
            return;
        }

        Vector3 dropPosition = HandTransform.position + transform.forward * HoldableItem.DropOffset.x + Vector3.up * HoldableItem.DropOffset.y;
        dropPosition.y -= 0.5f;
        Quaternion dropRotation = HandTransform.rotation;

        HoldableItem.UnHold(dropPosition, dropRotation);
        HoldableItem = null;

        animator.SetBool("Carry", false);

    }

    // 음식 버린후 초기화
    [PunRPC]
    private void DropFood()
    {
        if (HoldableItem != null)
        {
            if (HoldableItem is FoodObject || HoldableItem is DishObject)
            {
                FoodTrashDrop();
            }
        }


    }

    public void FoodTrashDrop()
    {
        if (HoldableItem == null)
        {
            return;
        }

        if (HoldableItem is FoodObject food)
        {
            food.Destroy();
            HoldableItem = null;
            animator.SetBool("Carry", false);
        }
        else if (HoldableItem.TryGetComponent<FoodCombination>(out var foodcombo))
        {
            foodcombo.Init();
            HoldableItem = null;
            animator.SetBool("Carry", false);
        }
    }

    [PunRPC]
    public void Place()
    {
        if (!IsHolding || HoldableItem == null)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f); // 예시로 1.0f 반경으로 체크
        foreach (Collider collider in colliders)
        {
            Stove stove = null;
            collider.TryGetComponent<Stove>(out stove);
            Table table = null;
            collider.TryGetComponent<Table>(out table); // Table 클래스를 기반으로 가정

            if (stove != null && stove.IsOnFire)
            {
                Debug.Log("Stove is on fire! Cannot place pan.");
                return;
            }
            else if (table != null && table.IsOnFire)
            {
                Debug.Log("Table is on fire! Cannot place pan.");
                return;
            }
        }

        //Quaternion placeRotation = Quaternion.identity;
        HoldableItem.Place(PlacePosition);
        HoldableItem = null;
        animator.SetBool("Carry", false);

    }


    public void SetNearTrashBin(bool value, Transform _carryTranform = null)
    {
        nearTrashBin = value;
        _carryTransform = _carryTranform;
    }

    public void SpawnPlateOnHand()
    {
        Character character = GetComponent<Character>();
        GameObject dishPrefab = Resources.Load<GameObject>("Plate_Combination");

        if (dishPrefab != null)
        {
            GameObject dish = PhotonNetwork.InstantiateRoomObject("Plate_Combination", HandTransform.position, HandTransform.rotation);

            IHoldable holdable = dish.GetComponent<IHoldable>();

            if (holdable != null)
            {
                holdable.Hold(character, HandTransform);
            }
        }
    }


    [PunRPC]
    public void RequestSpawnPlateOnHand()
    {
        Character nearbyCharacter = GetComponent<Character>();

        if (PhotonNetwork.IsMasterClient == false)
        {
            Debug.Log("방장이 아닌데 RequestSpawnPlateOnHand를 호출하려고 한다..");
            return;
        }

        GameObject dishPrefab = Resources.Load<GameObject>("Plate_Combination");

        if (dishPrefab != null)
        {
            GameObject dish = PhotonNetwork.InstantiateRoomObject("Plate_Combination", HandTransform.position, HandTransform.rotation);


            IHoldable holdable = dish.GetComponent<IHoldable>();
            if (holdable != null)
            {
                _pv.RPC(nameof(ResponseHold), RpcTarget.AllBuffered, dish.GetComponent<PhotonView>().ViewID, nearbyCharacter.PhotonView.ViewID);
            }
        }
    }


    public void SpawnDirtyPlateOnHand()
    {
        Character character = GetComponent<Character>();
        GameObject dishPrefab = Resources.Load<GameObject>("DirtyPlates");

        if (dishPrefab != null)
        {
            GameObject dish = PhotonNetwork.InstantiateRoomObject("DirtyPlates", HandTransform.position, HandTransform.rotation);

            IHoldable holdable = dish.GetComponent<IHoldable>();

            if (holdable != null)
            {
                holdable.Hold(character, HandTransform);
            }
        }
    }

    [PunRPC]
    public void RequestSpawnDirtyPlateOnHand()
    {
        Character nearbyCharacter = GetComponent<Character>();

        if (PhotonNetwork.IsMasterClient == false)
        {
            Debug.Log("방장이 아닌데 RequestSpawnDirtyPlateOnHand를 호출하려고 한다..");
            return;
        }

        GameObject dishPrefab = Resources.Load<GameObject>("DirtyPlates");

        if (dishPrefab != null)
        {
            GameObject dish = PhotonNetwork.InstantiateRoomObject("DirtyPlates", HandTransform.position, HandTransform.rotation);


            IHoldable holdable = dish.GetComponent<IHoldable>();
            if (holdable != null)
            {
                _pv.RPC(nameof(ResponseHold), RpcTarget.AllBuffered, dish.GetComponent<PhotonView>().ViewID, nearbyCharacter.PhotonView.ViewID);
            }
        }
    }

    [PunRPC]
    public void ResponseHold(int platePhotonViewID, int characterPhtonViewID)
    {
        PhotonView platePV = PhotonView.Find(platePhotonViewID);
        PhotonView characterPV = PhotonView.Find(characterPhtonViewID);

        if (platePV == null || characterPV == null)
        {
            return;
        }
        if (platePV.OwnerActorNr != characterPV.OwnerActorNr)
        {
            platePV.TransferOwnership(characterPV.OwnerActorNr);
        }
        IHoldable holdable = platePV.GetComponent<IHoldable>();
        Character character = characterPV.GetComponent<Character>();

        holdable.Hold(character, character.HoldAbility.HandTransform);
    }
}