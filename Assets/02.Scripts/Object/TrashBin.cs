using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pan"))
        {
            PanObject panObject = other.GetComponent<PanObject>();
            if (panObject != null)
            {
                panObject.SetNearTrashBin(true, this); 
            }
        }
        if (other.CompareTag("Player"))
        {
            CharacterHoldAbility holdAbility = other.GetComponent<CharacterHoldAbility>();
            if (holdAbility != null)
            {
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