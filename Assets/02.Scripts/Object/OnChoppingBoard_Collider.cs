using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChoppingBoard_Collider : MonoBehaviour
{
    [HideInInspector]
    public bool IsCuttable = false;

    public GameObject FoodOnBoard;
    public FoodObject FoodObject { get; private set; } // FoodObject를 가져오는 프로퍼티 추가

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            FoodObject foodObject = other.GetComponent<FoodObject>();
            if (foodObject.State == FoodState.Raw && foodObject.FoodType == FoodType.Lettuce)
            {
                IsCuttable = true;
                FoodOnBoard = other.gameObject;
                FoodObject = foodObject; // FoodObject 할당
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            FoodObject foodObject = other.GetComponent<FoodObject>();
            if (foodObject != null && foodObject == FoodOnBoard.GetComponent<FoodObject>())
            {
                IsCuttable = false;
                FoodOnBoard = null;
                FoodObject = null; // FoodObject 해제
            }
        }
    }
}
