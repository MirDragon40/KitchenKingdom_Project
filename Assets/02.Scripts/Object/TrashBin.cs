using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Pan"))
        {
            PanObject panObject = other.GetComponent<PanObject>();
            if (panObject != null)
            {
                Debug.Log("팬");
                panObject.SetNearTrashBin(true, this);
            }
        }
        else if (other.CompareTag("Basket"))
        {
            BasketObject basketObject = other.GetComponent<BasketObject>();
            if (basketObject != null)
            {
                basketObject.SetNearTrashBin(true, this);
            }
        }
        if (other.CompareTag("Player"))
        {
            CharacterHoldAbility holdAbility = other.GetComponent<CharacterHoldAbility>();
            if (holdAbility != null)
            {
                Debug.Log("플레이어");
                holdAbility.SetNearTrashBin(true, this.transform);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pan"))
        {
            PanObject panObject = other.GetComponent<PanObject>();
            if (panObject != null)
            {
                panObject.SetNearTrashBin(false);
            }
        }
        if (other.CompareTag("Player"))
        {
            CharacterHoldAbility holdAbility = other.GetComponent<CharacterHoldAbility>();
            if (holdAbility != null)
            {
                holdAbility.SetNearTrashBin(false);
            }
        }
    }
}