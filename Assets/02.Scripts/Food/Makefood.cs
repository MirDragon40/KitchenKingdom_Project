using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makefood : MonoBehaviour
{
    public static Makefood instace;

    public FoodType foodType;
    public Transform spawnPoint;



    private Character _nearbyCharacter;
    private bool isPlayerNearby => _nearbyCharacter != null;


    // 박스 열리는 애니메이션
    public Animator Animator;



    void Update()
    {
        if(!isPlayerNearby)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnFood(foodType, _nearbyCharacter.HoldAbility.handTransform);

            // 박스 애니메이션

            Animator.SetBool("PlayerBoxOpen", true);

            StartCoroutine(BoxOpenAnimation());
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
        if(isPlayerNearby)
        {
            return;
        }


        // 플레이어가 상자에 가까이 왔을 때
        if (other.CompareTag("Player"))
        {
            _nearbyCharacter = other.GetComponent<Character>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 상자에서 멀어졌을 때
        if (other.CompareTag("Player"))
        {
            var a = other.GetComponent<Character>();
            if (a == _nearbyCharacter)
            {
                _nearbyCharacter = null;
            }
        }
    }

    private IEnumerator BoxOpenAnimation()
    {
        yield return new WaitForSeconds(1f);
        Animator.SetBool("PlayerBoxOpen", false);
    }
}