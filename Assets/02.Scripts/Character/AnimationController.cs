using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    public void Carry()
    {
        animator.SetBool("Carry", true);
    }

    public void StopCarry()
    {
        animator.SetBool("Carry", false);
    }

}
