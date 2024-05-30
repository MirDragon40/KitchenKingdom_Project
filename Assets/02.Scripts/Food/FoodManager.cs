using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum FoodType
{
    Bread,
    Lettuce,
    Patty,
    Potato,
    Tomato,
    Cheese,
}

public class FoodManager : MonoBehaviour
{
    public static FoodManager instance;

    public GameObject[] foodPrefabs;
    //public Dictionary<FoodType, GameObject> foodPrefabContainer = new Dictionary<FoodType, GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
/*        for (int i = 0; i < foodPrefabs.Length; i++)
        {
            foodPrefabContainer.Add((FoodType)i, foodPrefabs[i]);
        }*/
    }
/*    public GameObject GetFoodPrefab(FoodType foodType)
    {
        return foodPrefabContainer[foodType];
    }*/
    public GameObject GetFoodPrefab(FoodType foodType)
    {
        int index = (int)foodType;
        if (index >= 0 && index < foodPrefabs.Length)
        {
            return foodPrefabs[index];
        }
        else
        {
            return null;
        }
    }
}
    
