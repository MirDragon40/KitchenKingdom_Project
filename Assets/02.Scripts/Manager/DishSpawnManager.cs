using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishSpawnManager : MonoBehaviour
{
   public static DishSpawnManager Instance;

    public GameObject Plate_Stage1_Object;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
