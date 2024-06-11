using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookStand : MonoBehaviourPun
{

    // 플레이어가 들고있는 음식을 PlacePosition위에 올려놓음
    public Transform PlacePosition;
    public bool IsOccupied { get; private set; }

    public GameObject PlacedItem;

    protected virtual void Update()
    {
        if (PlacePosition.childCount != 0)
        {
            PlacedItem = PlacePosition.transform.GetChild(0).gameObject;
        }
        else
        {
            PlacedItem = null;
        }

        if (PlacedItem != null)
        {
            IsOccupied = true;
        }
        else
        {
            IsOccupied= false;
        }

          /* if (_isPlaceable && Input.GetKeyDown(KeyCode.Space))
         {
             // 플레이어의 자식들 중에서 Food 태그를 가진 오브젝트를 찾음
             Transform foodObject = null;
             foreach (Transform child in player.transform)
             {
                 if (child.CompareTag("Food"))
                 {
                     foodObject = child;
                     break;
                 }
             }

             // Food 태그를 가진 오브젝트를 찾았을 때
             if (foodObject != null)
             {
                 // 오브젝트를 Table의 자식으로 설정
                 foodObject.SetParent(transform);

                 // 위치를 placePosition의 위치로 이동
                 foodObject.position = PlacePosition.transform.position;

                 // 필요한 경우 로컬 위치나 회전도 복사 가능
                 // foodObject.localPosition = placePosition.transform.localPosition;
                 // foodObject.localRotation = placePosition.transform.localRotation;
             }
             else
             {
                 Debug.Log("Food 태그를 가진 오브젝트를 찾을 수 없음.");
             }
         }
        */
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !IsOccupied)
        {
            CharacterHoldAbility characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
            if (characterHoldAbility != null)
            {
                characterHoldAbility.IsPlaceable = true;
                characterHoldAbility.PlacePosition = PlacePosition;
            }
        }

    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterHoldAbility characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
            if (characterHoldAbility != null)
            {
                characterHoldAbility.IsPlaceable = false;
                characterHoldAbility.PlacePosition = null;
            }
        }
    }


}
