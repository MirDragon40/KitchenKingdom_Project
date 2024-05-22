using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knife : MonoBehaviour
{
    private Animator _animator;

    public Slider slider;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {

        

        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0))
            {
               
            }
            else if (Input.GetMouseButton(0))
            {
                _animator.SetTrigger("Chopping");


            }
            else if (Input.GetMouseButtonUp(0))
            {
                _animator.SetTrigger("Chopping");
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.ResetTrigger("Chopping");
        }
    }
}

