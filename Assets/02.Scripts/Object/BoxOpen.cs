using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpen : MonoBehaviour
{
    public Animator Animator;

    private bool _isPlayerBox = false;

    private void Update()
    {
        if (_isPlayerBox) 
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Animator.SetBool("PlayerBoxOpen", true);

                StartCoroutine(BoxOpenAnimation());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _isPlayerBox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _isPlayerBox = false;
        }
    }
    private IEnumerator BoxOpenAnimation() 
    {
        yield return new WaitForSeconds(1f);
        Animator.SetBool("PlayerBoxOpen", false);
    }
}
