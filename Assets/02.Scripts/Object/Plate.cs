using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public MeshRenderer RendererThisMaterial;
    public Material ThisMaterial;
    public Material ChangeMaterial;


    private void Update()
    {
        /*if (Input.GetKeyUp(KeyCode.Escape))
        {
            PlateDirty();
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            PlateClean();
        }*/
    }

    // 접시 더러워질때
    public void PlateDirty() 
    {
        RendererThisMaterial.material = ChangeMaterial;
    }

    // 접시 깨끗해짐
    public void PlateClean() 
    {
        RendererThisMaterial.material = ThisMaterial;
    }
}
