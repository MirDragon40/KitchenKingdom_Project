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
    [Header("일반주문서 점수")]
    public int NormalOrderPoints = 25;

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
        Recipies["burgerCoke"] = new List<string> { "burger", "coke" };
        Recipies["burgerCokeFry"] = new List<string> { "burger", "coke","fry" };


    }
    void Update()
    {

        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            string orderName = "burger";
            _isGenerating = true;
            UI_Bilge newBill = MyScrollView.AddItem(3);
            newBill.OrderedFood = orderName;
            newBill.IngrediantsNameList = Recipies[orderName];
            GeneratedOrderList.Add(orderName);
        }
        
/*        if(Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            SubmitOrder("burger");
        }*/

        if (!_isGenerating && _orderCount < MaxOrderNumber)
        {
            _orderCount++;
            
            if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
            {
                int orderRandIndex = Random.Range(0, 10);
                if (orderRandIndex <= 8)
                {
                    _pv.RPC("GenerateOrderRPC", RpcTarget.All, "burger");
                }
                else if (orderRandIndex == 9)
                {
                    _pv.RPC("GenerateOrderRPC", RpcTarget.All, "burgerCoke");
                }
            }
        }
    }


    public void SubmitOrder(string submittedFood)
    {
        for (int i = 0; i<GeneratedOrderList.Count; i++)
        {
            if (GeneratedOrderList[i] == submittedFood)
            {
                GeneratedOrderList.RemoveAt(i);
                // 점수 더하기
                AddTotalScore(NormalOrderPoints);
                // UI 삭제
                MyScrollView.RemoveItem(submittedFood);
                break;
            }
        }

    }

    [PunRPC]
    public void AddTotalScore(int score)
    {
        GameManager.Instance.TotalScore += score;
        
    }


    [PunRPC]
    void GenerateOrderRPC(string order)
    {
        StartCoroutine(GenerateOrder(order));
    }

    private bool _isGenerating = false;
    IEnumerator GenerateOrder(string orderName)
    {
        yield return new WaitForSeconds(MinOrderTimeSpan);
        _isGenerating = true;
        UI_Bilge newBill = MyScrollView.AddItem(Recipies[orderName].Count);
        newBill.OrderedFood = orderName;
        newBill.IngrediantsNameList = Recipies[orderName];
        GeneratedOrderList.Add(orderName);
        float waitTime = Random.Range(MinOrderTimeSpan, MaxOrderTimeSpan);
        yield return new WaitForSeconds(waitTime);
        _isGenerating = false;
    }


}
