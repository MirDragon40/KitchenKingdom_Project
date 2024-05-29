using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    public Transform PlacePosition;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterHoldAbility characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
            if (characterHoldAbility != null)
            {
                characterHoldAbility.IsPlaceable = true;
                characterHoldAbility.PlacePosition = PlacePosition;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterHoldAbility characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
            if (characterHoldAbility != null)
            {
                characterHoldAbility.IsPlaceable = false;
                characterHoldAbility.PlacePosition = null;
            }
        }
    }

}

