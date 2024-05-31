using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FoodCombination : MonoBehaviour 
{
    public BoxCollider PlateActionCollider;
    public Dictionary<string, bool> Ingrediants = new Dictionary<string, bool>();
    public List<GameObject>IngrediantsObject = new List<GameObject>();
    public List<Image> UI_Image = new List<Image>();

    private void Awake()
    {
        foreach (GameObject ingrediant in IngrediantsObject)
        {
            ingrediant.SetActive(false);
        }
    }
}
