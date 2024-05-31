using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FoodCombination : MonoBehaviour 
{
    public int Stage = 1;
    public List<GameObject> AvailableIngrediants = new List<GameObject>(); // 스테이지별 가능한 재료조합 미리 list에 prefab으로 넣어놓음
    public List<Image> UI_FoodIcon = new List<Image>();
    public Dictionary<string, bool> Ingrediants = new Dictionary<string, bool>();
    public bool IsSubmitable;
    private IHoldable _holdableObject;


    private void Awake()
    {
        foreach (GameObject ingrediant in AvailableIngrediants)
        {
            ingrediant.SetActive(false);
        }
        foreach (var icon in UI_FoodIcon)
        {
            icon.gameObject.SetActive(false);
        }
        switch (Stage)
        {
            case 1:
                Ingrediants["bread"] = false;
                Ingrediants["patty"] = false;
                Ingrediants["lettuce"] = false;
                Ingrediants["coke"] = false;
                Ingrediants["fry"] = false;
                break;
            case 2:
                break;
            default:
                break;
        }
    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsSubmitable)
        {
            SubmitIngrediant(_holdableObject.GetComponent<FoodObject>());
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<CharacterHoldAbility>().HoldableItem != null)
        {
            _holdableObject = other.GetComponent<CharacterHoldAbility>().HoldableItem;

        }
        if (other.CompareTag("Player") && other.GetComponent<CharacterHoldAbility>().HoldableItem != null)
        {
            IsSubmitable = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsSubmitable = false;
        }
    }

    private void SubmitIngrediant(FoodObject submittedFood)
    {
        if (submittedFood.ItemType == EItemType.Food && submittedFood.FoodType == FoodType.Bread && !Ingrediants["bread"])
        {
            Ingrediants["bread"] = true;
            Destroy(submittedFood.gameObject);
            RefreshPlate();
        }
        if (submittedFood.ItemType == EItemType.Food && submittedFood.FoodType == FoodType.Lettuce && !Ingrediants["lettuce"] && submittedFood.State == FoodState.Cut)
        {
            Ingrediants["lettuce"] = true;
            Destroy(submittedFood.gameObject);
            RefreshPlate();
        }
        if (submittedFood.ItemType == EItemType.Coke && !Ingrediants["coke"])
        {
            Ingrediants["coke"] = true;
            Destroy(submittedFood.gameObject);
            RefreshPlate();
        }

    }
    private void RefreshPlate()
    {
        switch (Stage)
        {
            case 1:
                if (Ingrediants["bread"] == true)
                {
                    AvailableIngrediants[3].SetActive(true);
                    UI_FoodIcon[0].gameObject.SetActive(true);
                }
                if (Ingrediants["patty"] == true)
                {
                    AvailableIngrediants[1].SetActive(true);
                    UI_FoodIcon[2].gameObject.SetActive(true);
                }
                if (Ingrediants["lettuce"])
                {
                    UI_FoodIcon[1].gameObject.SetActive(true);
                    AvailableIngrediants[2].SetActive(true);
                }
                if (Ingrediants["coke"])
                {
                    UI_FoodIcon[3].gameObject.SetActive(true);
                    AvailableIngrediants[4].SetActive(true);
                }
                if (Ingrediants["fry"])
                {
                    AvailableIngrediants[5].SetActive(true);
                }
                break;
        }

    }
}
