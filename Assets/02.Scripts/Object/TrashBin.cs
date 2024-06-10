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
    }
}