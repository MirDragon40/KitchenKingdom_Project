using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
   protected Player _owner { get; private set; }

    protected void Awake()
    {
        _owner = GetComponentInParent<Player>();
    }
}
