using Photon.Pun;
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
    private PhotonView _pv;

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
        if (_pv == null)
        {
            _pv = other.GetComponent<PhotonView>();
        }
        if (other.CompareTag("Player") && _pv.IsMine)
        {
            if(!_isReached) 
            {
                RendererBoxMaterial.material = ChangeMaterial;
                RendererBoxCapMaterial.material = ChangeMaterial;
                RendererBoxPlaneMaterial.material = ChangeMaterial;
            }

            _isReached = true;
        }
        else
        {
            RendererBoxMaterial.material = BoxMaterial;
            RendererBoxCapMaterial.material = BoxCapMaterial;
            RendererBoxPlaneMaterial.material = BoxPlaneMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _pv.IsMine)
        {
            RendererBoxMaterial.material = BoxMaterial;
            RendererBoxCapMaterial.material = BoxCapMaterial;
            RendererBoxPlaneMaterial.material = BoxPlaneMaterial;
        }

        _isReached = false;
    }
}
