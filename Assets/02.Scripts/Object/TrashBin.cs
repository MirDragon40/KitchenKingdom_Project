using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private CharacterHoldAbility _nearbyCharacterHoldAbility;
    public Transform pantransform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _nearbyCharacterHoldAbility = other.GetComponent<CharacterHoldAbility>();
            if (_nearbyCharacterHoldAbility != null)
            {
                _nearbyCharacterHoldAbility.SetNearTrashBin(true, pantransform); // transform을 사용하여 현재 게임 오브젝트의 Transform을 전달합니다.
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_nearbyCharacterHoldAbility != null)
            {
                _nearbyCharacterHoldAbility.SetNearTrashBin(false);
                _nearbyCharacterHoldAbility = null;
            }
        }
    }

    void Update()
    {
        if (_nearbyCharacterHoldAbility != null && Input.GetKeyDown(KeyCode.Space))
        {
            DropFood();
            Debug.Log("00");
        }
    }

    private void DropFood()
    {
        if (!_nearbyCharacterHoldAbility.IsHolding)
        {
            return;
        }

        // 음식일 때만 Drop 메서드를 호출
        FoodObject foodObject = _nearbyCharacterHoldAbility.HoldableItem as FoodObject;
        if (foodObject != null)
        {
            _nearbyCharacterHoldAbility.FoodTrashDrop();

            // 플레이스 포지션의 자식 오브젝트 중 음식을 담고 있는 것을 찾아서 삭제
            for (int i = _nearbyCharacterHoldAbility.PlacePosition.childCount - 1; i >= 0; i--)
            {
                Transform child = _nearbyCharacterHoldAbility.PlacePosition.GetChild(i);
                FoodObject childFoodObject = child.GetComponent<FoodObject>();
                if (childFoodObject != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}