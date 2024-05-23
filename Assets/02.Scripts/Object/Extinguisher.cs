using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    public GameObject PowderEffect;

    private void Awake()
    {
        PowderEffect.SetActive(false);
    }

    private void Update()
    {
        
    }
}
