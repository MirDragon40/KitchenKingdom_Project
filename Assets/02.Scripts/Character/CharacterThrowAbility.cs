using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterThrowAbility : MonoBehaviour
{
    public GameObject ThrowingDirectionSprite;
    private CharacterHoldAbility _holdAbility;
    public IThrowable Throwable;
    public bool IsThrowable;


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
        if (_holdAbility.IsHolding && Throwable == null)
        {
            IsThrowable = _holdAbility.HoldableItem.TryGetComponent<IThrowable>(out Throwable);
        }
        else if (!_holdAbility.IsHolding)
        {
            IsThrowable = false;
            Throwable = null;
        }
        if (IsThrowable && Input.GetKey(KeyCode.LeftControl))
        {
            ThrowingDirectionSprite.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            ThrowingDirectionSprite.SetActive(false);
            PlayerThrow();
        }

    }
    private void PlayerThrow()
    {
        Throwable.ThrowObject(this.transform.forward);
    }
}
