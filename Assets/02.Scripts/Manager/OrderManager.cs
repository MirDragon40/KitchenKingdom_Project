using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class OrderManager : MonoBehaviourPun
{
    public static OrderManager Instance { get; private set; }
    private int _stage => GameManager.Instance.CurrentStage;
    [HideInInspector]
    public UI_BilgeScrollView MyScrollView;
    public float MinOrderTimeSpan = 3.0f;
    public float MaxOrderTimeSpan = 10f;
    private int _orderCount = 0;
    public int MaxOrderNumber = 5;

    [Header("일반주문서 점수")]
    public int NormalOrderPoints = 30;
    public int NormalPenaltyPoint = -10;
    [Header("VIP주문서 점수")]
    public int VIPOrderPoints = 50;
    public int VIPPenaltyPoint = -30;
    [Header("진상주문서 점수")]
    public int RudeOrderPoints = 20;
    public int RudePenaltyPoint = -50;

    public List<string> GeneratedOrderList = new List<string>();


    public Dictionary<string, List<string>> Recipies = new Dictionary<string, List<string>>();

    public Dictionary<string, int> FoodScores = new Dictionary<string, int>
{
    { "burger", 15 },
    { "burgerCoke", 30 },
    { "burgerCokeFry", 50 }
};

    private PhotonView _pv;


    public DirtyPlateStand DirtyPlateStand;


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

        Recipies["burger"] = new List<string> { "bread", "patty", "lettuce" };
        Recipies["burgerFry"] = new List<string> { "bread", "patty", "lettuce", "Fry" };
        Recipies["burgerCoke"] = new List<string> { "bread", "patty", "lettuce", "coke" };
        Recipies["burgerCokeFry"] = new List<string> { "bread", "patty", "lettuce", "coke", "fry" };
        Recipies["cokeFry"] = new List<string> {"coke", "fry" };
        Recipies["tomatoBurger"] = new List<string> { "bread", "patty", "tomato"};
        Recipies["tomatoBurgerCoke"] = new List<string> { "bread", "patty", "tomato", "coke"};
        Recipies["tomatoBurgerCokeFry"] = new List<string> { "bread", "patty", "tomato", "coke", "fry"};
        Recipies["tomatoBurgerFry"] = new List<string> { "bread", "patty", "tomato", "fry"};
        Recipies["cheeseBurger"] = new List<string> { "bread", "patty", "cheese"};
        Recipies["cheeseBurgerCoke"] = new List<string> { "bread", "patty", "cheese", "coke"};
        Recipies["cheeseBurgerCokeFry"] = new List<string> { "bread", "patty", "cheese", "coke", "fry"};
        Recipies["cheeseBurgerFry"] = new List<string> { "bread", "patty", "cheese", "fry"};



    }
    void Update()
    {

        /*        if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    string orderName = "burger";
                    _isGenerating = true;
                    UI_Bilge newBill = MyScrollView.AddItem(3);
                    newBill.OrderedFood = orderName;
                    newBill.IngrediantsNameList = Recipies[orderName];
                    GeneratedOrderList.Add(orderName);
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SubmitOrder("burger");
                }
        */

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartCoroutine(SpawnDirtyPlateRPC_Coroutine(3f));
        }


        if (!_isGenerating && GeneratedOrderList.Count < MaxOrderNumber && PhotonNetwork.IsMasterClient)
        {
            switch (_stage)
            {
                case 1:
                    int orderRandIndex = Random.Range(0, 10);
                    if (orderRandIndex <= 5)
                    {
                        _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burger");
                    }
                    else if (orderRandIndex > 5)
                    {
                        _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burgerCoke");
                    }
                    break;
                case 2:
                    orderRandIndex = Random.Range(0, 10);
                    if (orderRandIndex <= 3)
                    {
                        _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burger");
                    }
                    else if (orderRandIndex < 6)
                    {
                        _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burgerCoke");
                    }
                    else
                    {
                        _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burgerCokeFry");
                    }
                    break;
                case 3:
                    orderRandIndex = Random.Range(0, 12);
                    switch (orderRandIndex)
                    {
                        case 0:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burgerCokeFry");
                            break;
                        case 1:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burgerCoke");
                            break;
                        case 2:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burgerFry");
                            break;
                        case 3:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burger");
                            break;
                        case 4:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "tomatoBurgerCokeFry");
                            break;
                        case 5:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "tomatoBurgerCoke");
                            break;
                        case 6:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "tomatoBurgerFry");
                            break;
                        case 7:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "tomatoBurger");
                            break;
                        case 8:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "cheeseBurgerCokeFry");
                            break;
                        case 9:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "cheeseBurgerCoke");
                            break;
                        case 10:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "cheeseBurgerFry");
                            break;
                        case 11:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "cheeseBurger");
                            break;
                    }
                    break;

                case 4:
                    orderRandIndex = Random.Range(0, 17);
                    switch (orderRandIndex)
                    {
                        case 0:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burgerCokeFry");
                            break;
                        case 1:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burgerCoke");
                            break;
                        case 2:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burgerFry");
                            break;
                        case 3:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "burger");
                            break;
                        case 4:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "tomatoBurgerCokeFry");
                            break;
                        case 5:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "tomatoBurgerCoke");
                            break;
                        case 6:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "tomatoBurgerFry");
                            break;
                        case 7:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "tomatoBurger");
                            break;
                        case 8:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "cheeseBurgerCokeFry");
                            break;
                        case 9:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "cheeseBurgerCoke");
                            break;
                        case 10:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "cheeseBurgerFry");
                            break;
                        case 11:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "cheeseBurger");
                            break;
                        case 12:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "cokeFry");
                            break;
                        case 13:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "fry");
                            break;
                        case 14:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "chickenCokeFry");
                            break;
                        case 15:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "chickenCoke");
                            break;
                        case 16:
                            _pv.RPC("GenerateOrderRPC", RpcTarget.AllBuffered, "chicken");
                            break;
                    }
                    break;

            }
        }
    }


    [PunRPC]
    private void GenerateOrderRPC(string orderName)
    {
        StartCoroutine(GenerateOrder(orderName));
    }


    [PunRPC]
    public bool SubmitOrder(string submittedFood)
    {
        bool HasFoundMatchedItem = false;
        for (int i = 0; i < GeneratedOrderList.Count; i++)
        {
            if (GeneratedOrderList[i] == submittedFood)
            {
                HasFoundMatchedItem = true;
                GeneratedOrderList.RemoveAt(i);
                // 점수 더하기
                if (FoodScores.TryGetValue(submittedFood, out int score))
                {
                    AddTotalScore(score);
                }
                else
                {
                    AddTotalScore(NormalOrderPoints); // 기본 점수 추가
                }
                // UI 삭제
                MyScrollView.RemoveItem(submittedFood);
                break;
            }
        }
        if (!HasFoundMatchedItem)
        {
            MyScrollView.OnSubmitIncorrectPlateEffect();
        }
        return HasFoundMatchedItem;
    }

    [PunRPC]
    public void AddTotalScore(int score)
    {
        GameManager.Instance.TotalScore += score;

    }


    private bool _isGenerating = false;
    IEnumerator GenerateOrder(string orderName)
    {
        _isGenerating = true;
        yield return new WaitForSeconds(MinOrderTimeSpan);

        UI_Bilge newBill = MyScrollView.AddItem(Recipies[orderName].Count);
        newBill.OrderedFood = orderName;
        newBill.IngrediantsNameList = Recipies[orderName];
        GeneratedOrderList.Add(orderName);
        float waitTime = Random.Range(MinOrderTimeSpan, MaxOrderTimeSpan);
        yield return new WaitForSeconds(waitTime);
        _isGenerating = false;
    }

    public void RequestAddDirtyPlates()
    {
        StartCoroutine(SpawnDirtyPlateRPC_Coroutine(3f));
    }
    private IEnumerator SpawnDirtyPlateRPC_Coroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        PhotonView pv = DirtyPlateStand.GetComponent<PhotonView>();
        pv.RPC("UpdatePlateNum", RpcTarget.AllBuffered, DirtyPlateStand.DirtyPlateNum + 1);
    }

}
