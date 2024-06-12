using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CookingType
{
    CookStand,
    Stove,
    SodaMachine
}
public class MaterialColorChange: MonoBehaviour
{
    public CookingType CookType;
    public MeshRenderer RendererThisMaterial;
    public Material ThisMaterial;
    public Material ChangeMaterial;
    private PhotonView _pv;
    private bool _isReached = false;



    private void Start()
    {
        
    }

    void Update()
    {
        if (_isReached) 
        {
            // 임시
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RendererThisMaterial.material = ThisMaterial;

              
            }
            else if(Input.GetKeyDown(KeyCode.LeftControl) && CookType == CookingType.SodaMachine) 
            {
                RendererThisMaterial.material = ThisMaterial;
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
                RendererThisMaterial.material = ChangeMaterial;
                
            }
            _isReached = true;

        }
        
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player") && _pv.IsMine)
        {
            RendererThisMaterial.material = ThisMaterial;
        }

/*        if (_pv != null)
        {
            _pv = null;
        }*/
        _isReached = false;

        
    }
}
