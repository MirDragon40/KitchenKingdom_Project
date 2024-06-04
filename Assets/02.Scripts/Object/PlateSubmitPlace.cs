using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlateSubmitPlace : MonoBehaviour
{
    private FoodCombination _foodCombo;
    public bool IsServeable = false;
    public List<string> IngrediantsInDish = new List<string>();
    private CharacterHoldAbility _holdability;


    private void Update()
    {
        if (IsServeable && Input.GetKeyDown(KeyCode.Space))
        {
            string plateContent = string.Empty;
            if (_foodCombo.Ingrediants["burger"] && _foodCombo.Ingrediants["coke"] && _foodCombo.Ingrediants["fry"])
            {
                plateContent = "burgerCokeFry";
            }
            else if (_foodCombo.Ingrediants["burger"] && _foodCombo.Ingrediants["coke"])
            {
                plateContent = "burgerCoke";
            }
            else if (_foodCombo.Ingrediants["burger"])
            {
                plateContent = "burger";
            }

            OrderManager.Instance.SubmitOrder(plateContent);
            Destroy(_foodCombo.gameObject);
            _foodCombo = null;
        }
    }

    // todo : 손에 들고있는 plate에 맞는 음식을 제출했을때 ordermanager의 내용과 비교하여 gamemanager의 totalscore 25점 더하기
    // 
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<CharacterHoldAbility>(out _holdability))
        {
            if (_holdability.HoldableItem.TryGetComponent<FoodCombination>(out _foodCombo))
            {
                if (_foodCombo.IsReadyServe)
                {
                    IsServeable = true;

                }
                else
                {
                    IsServeable = false;
                }
            }
        }
        else
        {
            IsServeable = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IsServeable = false;
    }

}
