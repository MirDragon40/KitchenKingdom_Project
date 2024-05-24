using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterHoldAbility : CharacterAbility
{


    public Transform handTransform;
    private Animator animator;
    private float _findfood = 1f;



    private IHoldable _holdableItem;
    public bool IsHolding => _holdableItem != null;



    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!IsHolding)
            {
                PickUpFood();
            }
            else
            {
                DropFood();
            }
        }
    }
  
   
    public void PickUpFood()
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

                _holdableItem = holdable;
                holdable.Hold(_owner, transform);
                animator.SetBool("Carry", true);

                break;
            }

        }


    }

    void DropFood()
    {
        // 들고 있는 음식이 없으면 아무 작업도 수행하지 않음
        if (!IsHolding)
        {
            return;
        }

        // 부모 해제 전에 현재 위치와 회전을 저장
        Vector3 dropPosition = handTransform.position + transform.forward * 0.5f + Vector3.up * 0.8f; // 캐릭터 앞의 위치, 0.5f는 원하는 거리 조절
        dropPosition.y -= 1f;

        Quaternion dropRotation = handTransform.rotation; /** Quaternion.Euler(-90, 0, 0);*/ // 손의 회전 + 90도 회전

        _holdableItem.UnHold(dropPosition, dropRotation);
        _holdableItem = null;

        // 애니메이션 정지
        animator.SetBool("Carry", false);
    }
}