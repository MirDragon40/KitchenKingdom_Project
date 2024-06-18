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
    

    public PhotonView _pv;
    public Transform PlacePosition = null;

    private bool nearTrashBin = false;
    private Transform panTransform; // 팬 오브젝트를 참조하기 위한 변수

    public FireObject fireObject;
    void Start()
    {
        animator = GetComponent<Animator>();
        _pv = _owner.PhotonView;
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
                    }
                    else
                    {

                        _pv.RPC("Drop",RpcTarget.All);

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

        Debug.Log("PickUp");

        // 주변에 있는 잡을 수 있는 아이템을 찾음
        Collider[] colliders = Physics.OverlapSphere(transform.position, _findfood);

        foreach (Collider collider in colliders)
        {
            IHoldable holdable = collider.GetComponent<IHoldable>();
            if (holdable is PanObject pan)
            {
                // 스토브가 불이 났다면 팬을 잡을 수 없도록 함
                Stove stove = pan.GetComponentInParent<Stove>();
                if (stove != null && stove.IsOnFire)
                {
                    return;
                }

                Table[] nearbyTables = pan.NearbyTables;
                foreach (Table table in nearbyTables)
                {
                    Stove nearbyStove = table.GetComponentInParent<Stove>();
                    if (nearbyStove != null && nearbyStove.IsOnFire)
                    {
                        Debug.Log("Cannot pick up the pan as a nearby stove is on fire.");
                        return; // 팬을 들 수 없도록 반환
                    }
                }
            }

            if (holdable != null)
            {
               // Debug.Log("PickUp_Complete");

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
        if (!IsHolding || HoldableItem is PanObject)
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
        if (HoldableItem is FoodObject || HoldableItem is DishObject)
        {
            FoodTrashDrop();
        }

    }

    public void FoodTrashDrop()
    {
        if (HoldableItem == null)
        {
            return;
        }
        FoodCombination foodcombo = null;
        if (HoldableItem is FoodObject food)
        {
            food.Destroy();
            HoldableItem = null;
            animator.SetBool("Carry", false);

        }
        if (HoldableItem.TryGetComponent<FoodCombination>(out foodcombo))
        {
            HoldableItem.GetComponent<FoodCombination>().Init();
        }
    }

    [PunRPC]
    public void Place()
    {
        if (!IsHolding || HoldableItem == null)
        {
            return;
        }

        //Quaternion placeRotation = Quaternion.identity;
        HoldableItem.Place(PlacePosition);
        HoldableItem = null;
        animator.SetBool("Carry", false);
    }


    public void SetNearTrashBin(bool value, Transform pan = null)
    {
        nearTrashBin = value;
        panTransform = pan; // 팬 오브젝트 참조 설정
    }

    public void SpawnPlateOnHand()
    {
        Character character = GetComponent<Character>();
        GameObject dishPrefab = Resources.Load<GameObject>("Plate_Stage1");

        if (dishPrefab != null)
        {
            GameObject dish = PhotonNetwork.InstantiateRoomObject("Plate_Stage1", HandTransform.position, HandTransform.rotation);

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

        GameObject dishPrefab = Resources.Load<GameObject>("Plate_Stage1");

        if (dishPrefab != null)
        {
            GameObject dish = PhotonNetwork.InstantiateRoomObject("Plate_Stage1", HandTransform.position, HandTransform.rotation);


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

        IHoldable holdable = platePV.GetComponent<IHoldable>();
        Character character = characterPV.GetComponent<Character>();

        holdable.Hold(character, character.HoldAbility.HandTransform);
    }
}