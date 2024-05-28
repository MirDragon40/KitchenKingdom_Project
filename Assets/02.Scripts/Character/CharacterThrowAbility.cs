using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterThrowAbility : MonoBehaviour
{
    public GameObject ThrowingDirectionSprite;
    private CharacterHoldAbility _holdAbility;



    private void Awake()
    {
        _holdAbility = GetComponent<CharacterHoldAbility>();

    }
    void Start()
    {
        ThrowingDirectionSprite.SetActive(false);
    }

    void Update()
    {
        if (_holdAbility.IsHolding)
        {

        }
    }
}
