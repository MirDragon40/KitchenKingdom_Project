using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbility : MonoBehaviourPun
{
   protected Character _owner { get; private set; }

    protected virtual void Awake()
    {
        _owner = GetComponentInParent<Character>();
    }
}
