using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCoke : MonoBehaviour
{
    public GameObject CokePrefab;

    public Transform CokeSpawnPoint;

    private bool _isPlayerAround;

    public bool _isCokeOK = false;

    private void Update()
    {
        if (_isPlayerAround && Input.GetKeyDown(KeyCode.I)) 
        {
            
            if (!_isCokeOK) 
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

    private void SpawnCoke() 
    {
        Instantiate(CokePrefab, CokeSpawnPoint);
        _isCokeOK = true;
    }
}
