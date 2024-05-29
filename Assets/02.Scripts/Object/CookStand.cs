using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookStand : MonoBehaviour
{

    // 플레이어가 들고있는 음식을 PlacePosition위에 올려놓음
    public Transform PlacePosition;

    // 플레이어의 정보를 받아올 변수
    private GameObject player;

    private void Update()
    {
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterHoldAbility characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
            if (characterHoldAbility != null)
            {
                characterHoldAbility.IsPlaceable = true;
                characterHoldAbility.PlacePosition = PlacePosition;
            }
            player = other.gameObject; // 플레이어 오브젝트 저장
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterHoldAbility characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
            if (characterHoldAbility != null)
            {
                characterHoldAbility.IsPlaceable = false;
                characterHoldAbility.PlacePosition = null;
            }
            player = null; // 플레이어 오브젝트 해제
        }
    }
}
