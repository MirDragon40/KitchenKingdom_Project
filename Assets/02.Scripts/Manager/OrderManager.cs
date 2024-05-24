using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }
    [HideInInspector]
    public UI_BilgeScrollView MyScrollView;
    public float MinOrderTimeSpan = 3.0f;
    public float MaxOrderTimeSpan = 10f;
    private int _orderCount = 0;
    public int MaxOrderNumber = 5;
    public Dictionary<string, List<string>> Recipies = new Dictionary<string, List<string>>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);

        }
        MyScrollView = GameObject.FindObjectOfType<UI_BilgeScrollView>();
        Recipies["burger"] = new List<string> { "bread", "lettuce", "patty" };
    }
    void Update()
    {
        if (!isGenerating && _orderCount < MaxOrderNumber)
        {
            _orderCount++;
            StartCoroutine(GenerateOrder("burger"));
        }
    }

    private bool isGenerating = false;
    IEnumerator GenerateOrder(string orderName)
    {
        isGenerating = true;
        GameObject newItem = MyScrollView.AddItem();
        UI_Bilge newBill = newItem.GetComponent<UI_Bilge>();
        newBill.OrderedFood = orderName;
        newBill.IngrediantsNameList = Recipies[orderName];

        float waitTime = Random.Range(MinOrderTimeSpan, MaxOrderTimeSpan);
        yield return new WaitForSeconds(waitTime);


        isGenerating = false;
    }

    string OrderToString(string dish, List<string> ingredients)
    {
        string result = "Dish: " + dish + ", Ingredients: ";
        result += string.Join(", ", ingredients);
        return result;
    }

}
