using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxColorChange : MonoBehaviour
{
    public MeshRenderer RendererBoxMaterial;
    public MeshRenderer RendererBoxCapMaterial;
    public MeshRenderer RendererBoxPlaneMaterial;
    public Material BoxMaterial;
    public Material BoxCapMaterial;
    public Material BoxPlaneMaterial;

    public Material ChangeMaterial;

    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RendererBoxMaterial.material = ChangeMaterial;
            RendererBoxCapMaterial.material = ChangeMaterial;
            RendererBoxPlaneMaterial.material = ChangeMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RendererBoxMaterial.material = BoxMaterial;
            RendererBoxCapMaterial.material = BoxCapMaterial;
            RendererBoxPlaneMaterial.material = BoxPlaneMaterial;
        }
    }
}
