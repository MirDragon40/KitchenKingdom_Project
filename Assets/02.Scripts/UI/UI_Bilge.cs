using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Bilge : MonoBehaviour
{
    [HideInInspector]
    public string OrderedFood;
    [HideInInspector]
    public List<string> IngrediantsNameList = new List<string>();
    private bool _hasSpriteUpdated = false;
    public int ItemCount;

    public Image OrderFoodImage;
    public Image[] IngrediantImageSpace;
    public Image[] CookingMethodImageSpace;
    public Image[] PlusImageSpace;

    public Dictionary<string,Sprite> IngrediantSprites = new Dictionary<string, Sprite>();
    public Dictionary<string,Sprite> OrderSprites = new Dictionary<string, Sprite>();
    public Dictionary<string,Sprite> CookMethodSprites = new Dictionary<string, Sprite>();

    public List<Sprite> IngrediantSpriteList;
    public List<Sprite> OrderSpriteList;
    public List<Sprite> MethodSpriteList;

    private void Awake()
    {
        IngrediantSprites["bread"] = IngrediantSpriteList[0];
        IngrediantSprites["patty"] = IngrediantSpriteList[1];
        IngrediantSprites["lettuce"] = IngrediantSpriteList[2];
        IngrediantSprites["coke"] = IngrediantSpriteList[3];
        IngrediantSprites["fry"] = IngrediantSpriteList[4];

        CookMethodSprites["grill"] = MethodSpriteList[0];
        CookMethodSprites["fry"] = MethodSpriteList[1];

        OrderSprites["burger"] = OrderSpriteList[0];
        OrderSprites["burgerCoke"] = OrderSpriteList[1];
        OrderSprites["burgerCokeFry"] = OrderSpriteList[2];

        
    }

    private void Update()
    {
        if (!_hasSpriteUpdated)
        { 
            SpriteRefresh();
        }
    }
    private void SpriteRefresh()
    {
        for (int i=0;i<PlusImageSpace.Length;i++)
        {
            PlusImageSpace[i].enabled = false;
        }
        switch (ItemCount)
        {
            case 1:
                break;
            case 2:
                PlusImageSpace[0].enabled = true;
                break;
            case 3:
                PlusImageSpace[0].enabled = true;
                PlusImageSpace[1].enabled = true;
                break;
            case 4:
                PlusImageSpace[0].enabled = true;
                PlusImageSpace[1].enabled = true;
                PlusImageSpace[2].enabled = true;
                break;
            case 5:
                PlusImageSpace[0].enabled = true;
                PlusImageSpace[1].enabled = true;
                PlusImageSpace[2].enabled = true;
                PlusImageSpace[3].enabled = true;
                break;
            default:
                break;

        }
        if (OrderedFood != null)
        {
            switch (OrderedFood)
            {
                case "burger":
                {
                    OrderFoodImage.sprite = OrderSprites["burger"];
                    break;
                }
                case "burgerCoke":
                {
                    OrderFoodImage.sprite = OrderSprites["burgerCoke"];
                    break;
                }
                case "burgerCokeFry":
                {
                    OrderFoodImage.sprite = OrderSprites["burgerCokeFry"];
                    break;
                }
                default:
                    OrderFoodImage.sprite = IngrediantSprites["burger"];
                    break;
            }
            for (int i=0;i<IngrediantImageSpace.Length;i++)
            {
                IngrediantImageSpace[i].enabled = false;
                CookingMethodImageSpace[i].enabled = false;
            }

            for (int i = 0; i < IngrediantsNameList.Count; i++)
            {
                IngrediantImageSpace[i].enabled = true;
                IngrediantImageSpace[i].sprite = IngrediantSprites[IngrediantsNameList[i]];

                if (IngrediantsNameList[i] == "patty")
                {
                    CookingMethodImageSpace[i].enabled = true;
                    CookingMethodImageSpace[i].sprite = CookMethodSprites["grill"];
                }

                if (IngrediantsNameList[i] == "fry")
                {
                    CookingMethodImageSpace[i].enabled = true;
                    CookingMethodImageSpace[i].sprite = CookMethodSprites["fry"];
                }
            }
            _hasSpriteUpdated = true;
        }
    }
}
