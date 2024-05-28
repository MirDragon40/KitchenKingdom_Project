using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CookingType
{
    CookStand,
    Stove
}
public class MaterialColorChange: MonoBehaviour
{
    public CookingType CookType;
    public MeshRenderer RendererThisMaterial;
    public Material ThisMaterial;
    public Material ChangeMaterial;

    private bool _isReached = false;

    public Image PlusImageUI;

    public Coke Coke;

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

                // Stove 위에 플러스 이미지
                if (CookType == CookingType.Stove) 
                {
                    PlusImageUI.gameObject.SetActive(false);
                }

               // Coke.CokePour();
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
