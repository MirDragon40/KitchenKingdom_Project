using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    public GameObject PowderEffect;
    public bool IsHold = false;

    private void Awake()
    {
        PowderEffect.SetActive(false);
    }

    private void Update()
    {
        if (CharacterHoldAbility.instance.IsHolding )
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                PowderEffect.SetActive(true);

            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                PowderEffect.SetActive(false);
            }
        }
    }
}
