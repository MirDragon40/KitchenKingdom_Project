using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterThrowAbility : CharacterAbility
{
    public GameObject ThrowingDirectionSprite;
    private CharacterHoldAbility _holdAbility;
    public IThrowable Throwable;
    public bool IsThrowable;
    public float ThrowPower = 8f;


    protected override void Awake()
    {
        base.Awake();
        _holdAbility = _owner.HoldAbility;

    }
    void Start()
    {
        ThrowingDirectionSprite.SetActive(false);
    }
    private Rigidbody _rigid;
    void Update()
    {
        if (_holdAbility.IsHolding && Throwable == null)
        {
            IsThrowable = _holdAbility.HoldableItem.TryGetComponent<IThrowable>(out Throwable);
            if (IsThrowable)
            {
                bool isCoke = _holdAbility.HoldableItem.GetComponent<FoodObject>().ItemType == EItemType.Coke;
                if (isCoke)
                {
                    IsThrowable = false;
                }
            }
        }
        else if (!_holdAbility.IsHolding)
        {
            IsThrowable = false;
            Throwable = null;
        }
        if (IsThrowable && Input.GetKey(KeyCode.LeftControl) && _owner.PhotonView.IsMine)
        {
            ThrowingDirectionSprite.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && _owner.PhotonView.IsMine)
        {
            ThrowingDirectionSprite.SetActive(false);
            _owner.PhotonView.RPC("PlayerThrow",RpcTarget.All);

        }

    }
    [PunRPC]
    private void PlayerThrow()
    {

        if (Throwable != null)
        {
            Throwable.ThrowObject(transform.forward + new Vector3(0,0.4f, 0), ThrowPower);
            _owner.Animator.SetBool("Carry",false);
        }
        IsThrowable = false;
        Throwable = null;
        _holdAbility.HoldableItem = null;
    }
}
