using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlateSubmitPlace : MonoBehaviour
{
    private FoodCombination _foodCombo;
    public bool IsSubmitable = false;
    public List<string> IngrediantsInDish = new List<string>();

    private void Update()
    {
        if (IsSubmitable)
        {
            OrderManager.Instance.SubmitOrder(_foodCombo.Ingrediants.Keys.ToList());
        }
    }

    // todo : 손에 들고있는 plate에 맞는 음식을 제출했을때 ordermanager의 내용과 비교하여 gamemanager의 totalscore 25점 더하기
    // 
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<CharacterHoldAbility>().HoldableItem.TryGetComponent<FoodCombination>(out _foodCombo))
        {
            if (_foodCombo.IsReadyServe)
            {
                IsSubmitable = true;
                Debug.Log("Submitable");

            }
            else
            {
                IsSubmitable = false;
            }
        }
        else
        {
            IsSubmitable = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IsSubmitable = false;
    }

}
