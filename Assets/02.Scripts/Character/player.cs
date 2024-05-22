using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public static player instance;

    public Transform handTransform;
    private GameObject heldFood;
    private AnimationController animator;

    public void Awake()
    {
        instance = this;
        animator = GetComponent<AnimationController>();
    }

    void Update()
    {
        // "E" 키를 누르면 음식을 들거나 놓기
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

    public void SpawnFood(GameObject foodPrefab, Transform spawnPoint)
    {
        // 음식 생성
        GameObject food = Instantiate(foodPrefab, spawnPoint.position, spawnPoint.rotation);

        // 음식을 플레이어의 손 위치에 배치
        if (handTransform != null)
        {
            food.transform.parent = handTransform; // 손의 자식으로 설정
            food.transform.localPosition = Vector3.zero; // 손 위치에 맞게 조정
            food.transform.localRotation = Quaternion.identity; // 손 위치에 맞게 회전 초기화
        }
        else
        {
            Debug.LogError("Hand transform not assigned!");
            return;
        }

        // 음식을 들고 다니는 애니메이션 재생
        if (animator != null)
        {
            animator.Carry();
        }
        else
        {
            Debug.LogError("Animation controller not assigned!");
        }

        // 들고 있는 음식 설정
        heldFood = food;
    }

    void PickUpFood()
    {
        // 들고 있는 음식이 없으면 아무 작업도 수행하지 않음
        if (heldFood != null)
        {
            return;
        }

        // 주변에 있는 음식을 찾음
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f); // 적절한 반경 설정

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Food"))
            {
                // 찾은 음식을 플레이어의 손 위치로 이동시킴
                heldFood = collider.gameObject;
                heldFood.transform.position = handTransform.position;
                heldFood.transform.rotation = handTransform.rotation;
                heldFood.transform.parent = handTransform;

                // 음식을 들고 다니는 애니메이션 재생
                animator.Carry();
                break;
            }
        }
    }

    void DropFood()
    {
        // 들고 있는 음식이 없으면 아무 작업도 수행하지 않음
        if (heldFood == null)
        {
            return;
        }

        heldFood.transform.parent = null; // 부모 해제
        heldFood.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z); // 플레이어 근처에 놓음
        heldFood = null; // 들고 있는 음식 초기화

        // 음식을 들고 다니는 애니메이션 정지
        animator.StopCarry();
    }
}