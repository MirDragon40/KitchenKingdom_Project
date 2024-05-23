using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makefood : MonoBehaviour
{
    public static Makefood instace;

    public FoodType foodType;
    public Transform spawnPoint;  
    private bool isPlayerNearby = false; // 박스근처 확인


    // 박스 열리는 애니메이션
    public Animator Animator;

    private bool _isPlayerBox = false;


    void Update()
    {
        if (_isPlayerBox) 
        {
            if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !CharacterHoldAbility.instance.IsHoldingFood())
            {
                SpawnFood(foodType, CharacterHoldAbility.instance.handTransform);

                // 박스 애니메이션

                Animator.SetBool("PlayerBoxOpen", true);

                StartCoroutine(BoxOpenAnimation());
            }
        }
        
    }

    public void SpawnFood(FoodType foodType, Transform spawnPoint)
    {
        // 음식 생성
        Debug.Log("음식생성");
        GameObject foodPrefab = FoodManager.instance.GetFoodPrefab(foodType);
        if (foodPrefab != null)
        {
            GameObject food = Instantiate(foodPrefab, spawnPoint.position, spawnPoint.rotation);

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 상자에 가까이 왔을 때
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            // 애니메이션
            _isPlayerBox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 상자에서 멀어졌을 때
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // 애니메이션
            _isPlayerBox = false;
        }
    }

    private IEnumerator BoxOpenAnimation()
    {
        yield return new WaitForSeconds(1f);
        Animator.SetBool("PlayerBoxOpen", false);
    }
}