using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makefood : MonoBehaviour
{
    public static Makefood instace;

    public FoodType foodType;
    public Transform spawnPoint;  
    private bool isPlayerNearby = false; // 박스근처 확인

    public void Awake()
    {
        
    }
    private void Start()
    {

    }
    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !Player.instance.IsHoldingFood())
        {
            if (Player.instance != null)
            {
                Player.instance.SpawnFood(foodType, spawnPoint);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 상자에 가까이 왔을 때
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 상자에서 멀어졌을 때
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}