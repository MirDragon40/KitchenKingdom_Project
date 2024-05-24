using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private CharacterHoldAbility _nearbyCharacterHoldAbility;

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 가까워짐
        if (other.CompareTag("Player"))
        {
            _nearbyCharacterHoldAbility = other.GetComponent<CharacterHoldAbility>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어 멀어짐
        if (other.CompareTag("Player"))
        {
            _nearbyCharacterHoldAbility = null;
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

        FoodObject food = _nearbyCharacterHoldAbility.GetComponentInChildren<FoodObject>();
        if (food != null)
        {
            food.Dispose();
            _nearbyCharacterHoldAbility.FoodTrashDrop();
        }
    }
}