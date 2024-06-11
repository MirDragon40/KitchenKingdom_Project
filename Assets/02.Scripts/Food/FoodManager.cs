using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public static FoodManager instance;

    public FoodType type;

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
    }

    public string GetFoodName(FoodType foodType)
    {
        return foodType.ToString();
    }
}
