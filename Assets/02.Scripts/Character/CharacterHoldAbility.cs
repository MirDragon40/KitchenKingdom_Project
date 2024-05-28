using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterHoldAbility : CharacterAbility
{


    public Transform handTransform;
    private Animator animator;
    private float _findfood = 1f; //음식을 찾는 범위

    [HideInInspector]
    public IHoldable HoldableItem;
   // private Transform _placeableSurface;

    public bool IsPlaceable =false;
    public bool IsHolding => HoldableItem != null;

    public Transform PlacePosition = null;



    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        /*        if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!IsHolding)
                    {
                        PickUp();
                    }
                    else
                    {
                        Drop();
                    }
                }*/
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
                    Drop();
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

        // 부모 해제 전에 현재 위치와 회전을 저장
        Vector3 dropPosition = handTransform.position + transform.forward * HoldableItem.DropOffset.x + Vector3.up * HoldableItem.DropOffset.y;
        dropPosition.y -= 0.5f;
        Quaternion dropRotation = handTransform.rotation; /** Quaternion.Euler(-90, 0, 0);*/ // 손의 회전 + 90도 회전

        HoldableItem.UnHold(dropPosition, dropRotation);


         HoldableItem = null;

        // 애니메이션 정지
        animator.SetBool("Carry", false);
    }

    // 음식 버린후 초기화
    public void FoodTrashDrop()
    {
        HoldableItem = null;
        animator.SetBool("Carry", false);
    }

    void Place()
    {
        if (!IsHolding )
        {
            return;
        }


        //Vector3 placePosition = transform.position + transform.forward * 0.5f + Vector3.up * 0.2f; 
        Quaternion placeRotation = Quaternion.identity;

        HoldableItem.Place(PlacePosition.position, placeRotation);
        HoldableItem = null;
        animator.SetBool("Carry", false);
    }
}