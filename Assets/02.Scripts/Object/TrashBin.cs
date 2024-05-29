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
                _nearbyCharacterHoldAbility.SetNearTrashBin(true);
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
        }
    }
}