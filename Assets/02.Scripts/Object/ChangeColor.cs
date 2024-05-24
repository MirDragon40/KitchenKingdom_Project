using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public MeshRenderer RendererThisMaterial;
    public Material ThisMaterial;
    public Material ChangeMaterial;

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            RendererThisMaterial.material = ChangeMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RendererThisMaterial.material = ThisMaterial;
        }
    }
}
