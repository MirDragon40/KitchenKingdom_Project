using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makefood : MonoBehaviour
{
    public GameObject foodPrefab; // 음식 프리팹
    public Transform spawnPoint;  // 음식이 생성될 위치
    private bool isPlayerNearby = false; // 플레이어가 근처에 있는지 여부

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            player.instance.SpawnFood(foodPrefab, spawnPoint);
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