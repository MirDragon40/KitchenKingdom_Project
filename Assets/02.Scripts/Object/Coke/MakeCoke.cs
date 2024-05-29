using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCoke : MonoBehaviour
{
    public GameObject CokePrefab;

    public Transform CokeSpawnPoint;

    private bool _isPlayerAround;

    private void Update()
    {
        if (_isPlayerAround && Input.GetKeyDown(KeyCode.LeftControl)) 
        {
            if(CokeSpawnPoint.childCount == 0) 
            {
                SpawnCoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _isPlayerAround = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _isPlayerAround = false;
        }
    }

    private void SpawnCoke() 
    {
        Instantiate(CokePrefab, CokeSpawnPoint);
    }
}
