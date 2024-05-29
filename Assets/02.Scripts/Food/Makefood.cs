using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makefood : MonoBehaviour
{

    public FoodType foodType;

    public Transform spawnPoint;

    private Character _nearbyCharacter;
    private bool isPlayerNearby => _nearbyCharacter != null;


    public IHoldable _placedItem;
    public bool HavePlacedItem => _placedItem != null;

    private Transform handTransform;

    // 박스 열리는 애니메이션
    public Animator animator;

    private float checkRange = 1f;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isPlayerNearby)
        {
            return;
        }

        if (_nearbyCharacter.HoldAbility.IsHolding)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!HavePlacedItem)
            {
                // 음식을 생성하기 전에 근처에 들 수 있는 오브젝트가 있는지 확인합니다
                if (!IsNearbyHoldable())
                {
                    SpawnFood(foodType, _nearbyCharacter.HoldAbility.handTransform);

                    // 들기 애니메이션 실행
                    _nearbyCharacter.GetComponent<Animator>().SetBool("Carry", true);

                    // 상자 애니메이션 실행
                    // Animator.SetBool("PlayerBoxOpen", true);

                    StartCoroutine(BoxOpenAnimation());
                }
            }
        }

        if (!_nearbyCharacter.HoldAbility.IsHolding && Input.GetKeyDown(KeyCode.Space))
        {
            _nearbyCharacter = null;
        }

    }

    private bool IsNearbyHoldable()
    {
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, checkRange);
        foreach (Collider collider in colliders)
        {
            IHoldable holdable = collider.GetComponent<IHoldable>();
            if (holdable != null)
            {
                return true; // 근처에 들 수 있는 오브젝트가 있음
            }
        }
        return false; // 근처에 들 수 있는 오브젝트가 없음
    }

    public void SpawnFood(FoodType foodType, Transform spawnPoint)
    {
        // 음식 생성
        
        GameObject foodPrefab = FoodManager.instance.GetFoodPrefab(foodType);
        Debug.Log(foodPrefab.transform.position);
        if (foodPrefab != null)
        {
            Debug.Log("음식 생성");
            GameObject food = Instantiate(foodPrefab, spawnPoint.position, spawnPoint.rotation);

            // 음식 오브젝트를 손에 들도록 설정
            IHoldable holdable = food.GetComponent<IHoldable>();
            if (holdable != null)
            {
                holdable.Hold(_nearbyCharacter, handTransform);
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (isPlayerNearby)
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
        //Animator.SetBool("PlayerBoxOpen", false);
    }
}