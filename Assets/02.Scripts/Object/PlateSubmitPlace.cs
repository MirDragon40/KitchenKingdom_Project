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
    private string _plateContent = string.Empty;


    private void LateUpdate()
    {
        if (IsServeable && Input.GetKeyDown(KeyCode.Space))
        {

                Debug.Log(_plateContent);
                OrderManager.Instance.SubmitOrder(_plateContent);
                Destroy(_foodCombo.gameObject);
                _foodCombo = null;
                _plateContent = string.Empty;
            

        }
    }

    // todo : 손에 들고있는 plate에 맞는 음식을 제출했을때 ordermanager의 내용과 비교하여 gamemanager의 totalscore 25점 더하기
    // 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<CharacterHoldAbility>(out _holdability))
        {
            if (_holdability.HoldableItem != null && _plateContent == string.Empty)
            {
                if (_holdability.HoldableItem.TryGetComponent<FoodCombination>(out _foodCombo))
                {
                    if (_foodCombo.IsReadyServe)
                    {
                        if (_foodCombo.Ingrediants["burger"] && _foodCombo.Ingrediants["coke"] && _foodCombo.Ingrediants["fry"])
                        {
                            _plateContent = "burgerCokeFry";
                        }
                        else if (_foodCombo.Ingrediants["burger"] && _foodCombo.Ingrediants["coke"])
                        {
                            _plateContent = "burgerCoke";
                        }
                        else if (_foodCombo.Ingrediants["burger"])
                        {
                            _plateContent = "burger";
                        }
                        IsServeable = true;
                        _holdability.IsServeable = true;

                    }
                    else
                    {
                        IsServeable = false;
                        _holdability.IsServeable = true;
                    }
                }

            }
        }
        else
        {
            IsServeable = false;
            _holdability.IsServeable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IsServeable = false;
        if (_holdability != null)
        {
            _holdability.IsServeable = false;
        }
    }

}
