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

    private bool _isReached = false;

    public Image PlusImageUI;

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

                // Stove 위에 플러스 이미지
                if (CookType == CookingType.Stove) 
                {
                    PlusImageUI.gameObject.SetActive(false);
                }
            }
            else if(Input.GetKeyDown(KeyCode.LeftControl) && CookType == CookingType.SodaMachine) 
            {
                RendererThisMaterial.material = ThisMaterial;
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
