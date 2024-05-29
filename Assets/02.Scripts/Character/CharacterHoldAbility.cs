using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterHoldAbility : CharacterAbility
{


    public Transform handTransform;
    private Animator animator;
    private float _findfood = 1f; //음식을 찾는 범위

    public IHoldable HoldableItem;
   // private Transform _placeableSurface;

    public bool IsPlaceable = false;
    public bool IsHolding => HoldableItem != null;

    public Transform PlacePosition = null;

    private bool nearTrashBin = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!IsHolding)
            {
                PickUp();
            }
            else
            {
                if (IsPlaceable)
                {
                    Place();
                }
                else
                {
                    if (nearTrashBin)
                    {
                        DropFood();
                    }
                    else
                    {
                        Drop();
                    }
                }
            }
        }
    }


    public void PickUp()
    {
        // 들고 있는 음식이 있으면 아무 작업도 수행하지 않음
        if (IsHolding)
        {
            return;
        }
       

        // 주변에 있는 잡을 수 있는 아이템을 찾음
        Collider[] colliders = Physics.OverlapSphere(transform.position, _findfood);

        foreach (Collider collider in colliders)
        {
            IHoldable holdable = collider.GetComponent<IHoldable>();
            if(holdable != null)
            {

                HoldableItem = holdable;
                holdable.Hold(_owner, transform);
                animator.SetBool("Carry", true);

                break;
            }

        }


    }

    void Drop()
    {
        // 들고 있는 음식이 없으면 아무 작업도 수행하지 않음
        if (!IsHolding)
        {
            return;
        }

        Vector3 dropPosition = handTransform.position + transform.forward * HoldableItem.DropOffset.x + Vector3.up * HoldableItem.DropOffset.y;
        dropPosition.y -= 0.5f;
        Quaternion dropRotation = handTransform.rotation;

        HoldableItem.UnHold(dropPosition, dropRotation);
        HoldableItem = null;

        animator.SetBool("Carry", false);

    }
    // 음식 버린후 초기화

    private void DropFood()
    {
        if (HoldableItem is FoodObject)
        {
            FoodTrashDrop();
        }
    }
    public void FoodTrashDrop()
    {
        if (HoldableItem is FoodObject food)
        {
            food.Destroy();
            HoldableItem = null;
            animator.SetBool("Carry", false);
        }
    }


    void Place()
    {
        if (!IsHolding )
        {
            return;
        }

        //Quaternion placeRotation = Quaternion.identity;
        HoldableItem.Place(PlacePosition);
        HoldableItem = null;
        animator.SetBool("Carry", false);
    }
    public void SetNearTrashBin(bool value)
    {
        nearTrashBin = value;
    }

}