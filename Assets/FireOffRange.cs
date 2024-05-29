using UnityEngine;

public class FireOffRange : MonoBehaviour
{
    public Extinguisher extinguisher;
    public void OnTriggerStay(Collider other)
    {
        Debug.Log(00);
        extinguisher.A(other);
    }

}
