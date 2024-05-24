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

    private bool _isReached = false;

    void Update()
    {
        if (_isReached)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                RendererBoxMaterial.material = BoxMaterial;
                RendererBoxCapMaterial.material = BoxCapMaterial;
                RendererBoxPlaneMaterial.material = BoxPlaneMaterial;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!_isReached) 
            {
                RendererBoxMaterial.material = ChangeMaterial;
                RendererBoxCapMaterial.material = ChangeMaterial;
                RendererBoxPlaneMaterial.material = ChangeMaterial;
            }

            _isReached = true;
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

        _isReached = false;
    }
}
