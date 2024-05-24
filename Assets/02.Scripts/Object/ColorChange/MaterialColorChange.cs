using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialColorChange: MonoBehaviour
{
    public MeshRenderer RendererThisMaterial;
    public Material ThisMaterial;
    public Material ChangeMaterial;

    private bool _isReached = false;

    public Image PlusImageUI;

    private void Start()
    {
        
    }

    void Update()
    {
        if (_isReached) 
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                RendererThisMaterial.material = ThisMaterial;

                PlusImageUI.gameObject.SetActive(false);
            }
        }
        
    }

    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
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
        if (other.CompareTag("Player"))
        {
            RendererThisMaterial.material = ThisMaterial;
        }

        _isReached = false;
    }
}
