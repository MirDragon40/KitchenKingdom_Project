using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class OrderManager : MonoBehaviourPun
{
    public static OrderManager Instance { get; private set; }
    [HideInInspector]
    public UI_BilgeScrollView MyScrollView;
    public float MinOrderTimeSpan = 3.0f;
    public float MaxOrderTimeSpan = 10f;
    private int _orderCount = 0;
    public int MaxOrderNumber = 5;

    public List<string> GeneratedOrderList = new List<string>();


    public Dictionary<string, List<string>> Recipies = new Dictionary<string, List<string>>();
    private PhotonView _pv;


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
        _pv = GetComponent<PhotonView>();
        MyScrollView = GameObject.FindObjectOfType<UI_BilgeScrollView>();
        Recipies["burger"] = new List<string> { "bread", "lettuce", "patty" };
    }
    void Update()
    {
        if (!isGenerating && _orderCount < MaxOrderNumber)
        {
            _orderCount++;
            if (PhotonNetwork.IsMasterClient)
            {
                _pv.RPC("GenerateOrderRPC", RpcTarget.All, "burger");
            }
        }
    }

    public void SubmitOrder(List<string> ingrediantsInDish)
    {
        foreach (string item in ingrediantsInDish)
        {
            foreach (string order in GeneratedOrderList)
            {
                if (item == order)
                {
                    Debug.Log("order matched!");
                    break;
                }
            }
        }
    }

    [PunRPC]
    void GenerateOrderRPC(string order)
    {
        StartCoroutine(GenerateOrder(order));
    }

    private bool isGenerating = false;
    IEnumerator GenerateOrder(string orderName)
    {
        isGenerating = true;
        GameObject newItem = MyScrollView.AddItem();
        UI_Bilge newBill = newItem.GetComponent<UI_Bilge>();
        newBill.OrderedFood = orderName;
        newBill.IngrediantsNameList = Recipies[orderName];
        GeneratedOrderList.Add(orderName);
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
