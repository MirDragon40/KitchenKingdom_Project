using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChoppingBoard_Collider : MonoBehaviour
{
    [HideInInspector]
    public bool IsCuttable = false;

    public GameObject FoodOnBoard;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            FoodObject foodObject = other.GetComponent<FoodObject>();
            if (foodObject.State == FoodState.Raw && foodObject.FoodType == FoodType.Lettuce)
            {
                IsCuttable = true;

                FoodOnBoard = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            FoodObject foodObject = other.GetComponent<FoodObject>();
            IsCuttable = false;
            
            FoodOnBoard = null; 
        }
    }
}
