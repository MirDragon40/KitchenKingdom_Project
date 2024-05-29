using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCoke : MonoBehaviour
{
    public GameObject CokePrefab;

    public Transform CokeSpawnPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCoke();
        }
    }
    private void SpawnCoke() 
    {
        Instantiate(CokePrefab, CokeSpawnPoint);
    }

}
