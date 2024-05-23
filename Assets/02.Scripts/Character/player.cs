using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public Transform handTransform;
    public GameObject heldFood;
    private Animator animator;
    private float _findfood = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldFood == null)
            {
                PickUpFood();
            }
            else
            {
                DropFood();
            }
        }

    }
    public bool IsHoldingFood()
    {
        return heldFood != null;
    }
   
    public void PickUpFood()
    {
     
        // 들고 있는 음식이 없으면 아무 작업도 수행하지 않음
        if (heldFood != null)
        {
            return;
        }

        // 주변에 있는 음식을 찾음
        Collider[] colliders = Physics.OverlapSphere(transform.position, _findfood);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Food"))
            {
                // 찾은 음식을 플레이어의 손 위치로 이동시킴
                heldFood = collider.gameObject;
                heldFood.transform.parent = handTransform;
                Debug.Log("food의 부모 " + heldFood.transform.parent);
                heldFood.transform.localPosition = Vector3.zero; ;
                heldFood.transform.localRotation = Quaternion.Euler(-90,0, 0);


                // 음식을 들고 다니는 애니메이션 재생
                animator.SetBool("Carry", true);
   

                break;
            }
        }
    }

    void DropFood()
    {
        Debug.Log(heldFood.transform.parent);
        // 부모 해제
        heldFood.transform.parent = null;

        // 들고 있는 음식이 없으면 아무 작업도 수행하지 않음
        if (heldFood == null)
        {
            return;
        }

        // 부모 해제 전에 현재 위치와 회전을 저장
        Vector3 dropPosition = handTransform.position + transform.forward * 0.5f + Vector3.up * 0.8f; // 캐릭터 앞의 위치, 0.5f는 원하는 거리 조절
        dropPosition.y -= 1f;

        Quaternion dropRotation = handTransform.rotation * Quaternion.Euler(-90, 0, 0); // 손의 회전 + 90도 회전


        // 저장한 위치와 회전으로 음식 배치
        heldFood.transform.position = dropPosition;
        heldFood.transform.rotation = dropRotation;




        // 애니메이션 정지
        animator.SetBool("Carry", false);
  

        // 음식 초기화
        heldFood = null;
    }
}