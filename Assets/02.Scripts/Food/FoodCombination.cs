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
    public bool IsReadyServe = false;

    public void Awake()
    {
    }
    private void Start()
    {
        Stage = GameManager.Instance.Stage;
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
                Ingrediants["burger"] = false;

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
            FoodObject ingrediant = null;
            if (_holdableObject.TryGetComponent<FoodObject>(out ingrediant))
            {
                SubmitIngrediant(ingrediant);
                Debug.Log(ingrediant);
            }
            else
            {
                SubmitIngrediant(_holdableObject.GetComponent<PanObject>().GrillingIngrediant.GetComponent<FoodObject>());
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<CharacterHoldAbility>().HoldableItem != null)
            {
                _holdableObject = other.GetComponent<CharacterHoldAbility>().HoldableItem;
                IsSubmitable = true;
            }
        }
        else if (!GetComponent<BoxCollider>().enabled)
        {
            IsSubmitable = false;
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
        else if (submittedFood.ItemType == EItemType.Food && submittedFood.FoodType == FoodType.Lettuce && !Ingrediants["lettuce"] && submittedFood.State == FoodState.Cut)
        {
            Ingrediants["lettuce"] = true;
            Destroy(submittedFood.gameObject);
            RefreshPlate();
        }
        else if (submittedFood.ItemType == EItemType.Coke && !Ingrediants["coke"])
        {
            Ingrediants["coke"] = true;
            Destroy(submittedFood.gameObject);
            RefreshPlate();
        }
        else if (submittedFood.ItemType == EItemType.Food && !Ingrediants["patty"] && submittedFood.State == FoodState.Grilled)
        {
            Ingrediants["patty"] = true;
            Destroy(submittedFood.gameObject);
            RefreshPlate();
        }

    }
    private void RefreshPlate()
    {
        switch (Stage)
        {
            case 1: // stage 1
                if (Ingrediants["bread"] && Ingrediants["patty"] && Ingrediants["lettuce"])
                {
                    AvailableIngrediants[0].SetActive(true);
                    AvailableIngrediants[1].SetActive(true);
                    AvailableIngrediants[2].SetActive(true);
                    AvailableIngrediants[3].SetActive(true);
                    Ingrediants["burger"] = true;

                    IsReadyServe = true;
                    UI_FoodIcon[0].gameObject.SetActive(false);
                    UI_FoodIcon[1].gameObject.SetActive(false);
                    UI_FoodIcon[2].gameObject.SetActive(false);
                    UI_FoodIcon[5].gameObject.SetActive(true);
                }
                if (!Ingrediants["burger"])
                {
                    if (Ingrediants["bread"])
                    {
                        AvailableIngrediants[0].SetActive(true);
                        AvailableIngrediants[3].SetActive(true);
                        UI_FoodIcon[0].gameObject.SetActive(true);
                    }
                    if (Ingrediants["patty"])
                    {
                        AvailableIngrediants[1].SetActive(true);
                        UI_FoodIcon[2].gameObject.SetActive(true);
                    }
                    if (Ingrediants["lettuce"])
                    {
                        AvailableIngrediants[2].SetActive(true);
                        UI_FoodIcon[1].gameObject.SetActive(true);
                    }
                }

                if (Ingrediants["coke"])
                {
                    UI_FoodIcon[3].gameObject.SetActive(true);
                    AvailableIngrediants[4].SetActive(true);
                }
                if (Ingrediants["fry"])
                {
                    UI_FoodIcon[4].gameObject.SetActive(true);
                    AvailableIngrediants[5].SetActive(true);
                }
                break;
        }

    }
}
