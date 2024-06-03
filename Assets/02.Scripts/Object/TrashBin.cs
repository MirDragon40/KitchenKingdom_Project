using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private CharacterHoldAbility _nearbyCharacterHoldAbility;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _nearbyCharacterHoldAbility = other.GetComponent<CharacterHoldAbility>();
            if (_nearbyCharacterHoldAbility != null)
            {
                _nearbyCharacterHoldAbility.SetNearTrashBin(true, transform); // transform을 사용하여 현재 게임 오브젝트의 Transform을 전달합니다.
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
        }
    }

    private void DropFood()
    {
        if (!_nearbyCharacterHoldAbility.IsHolding)
        {
            return;
        }

        // 음식일 때만 Drop 메서드를 호출
        if (_nearbyCharacterHoldAbility.HoldableItem is FoodObject)
        {
            _nearbyCharacterHoldAbility.FoodTrashDrop();

            // 팬(Transform)의 자식 오브젝트 중 음식을 담고 있는 것을 찾아서 삭제
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.CompareTag("Food"))
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}