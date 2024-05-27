using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateColorChange : MonoBehaviour
{
    public MeshRenderer RendererThisMaterial;
    public Material ThisMaterial;
    public Material ChangeMaterial;

    private void Update()
    {
        
    }


    // 접시 더러워질때
    public void PlateDirty() 
    {
        RendererThisMaterial.material = ChangeMaterial;
    }
}
