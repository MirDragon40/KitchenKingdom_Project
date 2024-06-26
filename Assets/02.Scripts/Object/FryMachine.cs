using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryMachine : CookStand
{
    public BasketObject PlacedBasket = null;
    private Transform _originalPlacePosition;
    public FireObject fireObject;
    public SoundManager SoundManager;
    public bool IsBasketPlaced => PlacedBasket != null;

    private bool isFireExtinguished = true;
    private Coroutine igniteCoroutine;
    public Table[] NearbyTables;
    private void Awake()
    {
        fireObject = GetComponent<FireObject>();
        SoundManager = FindObjectOfType<SoundManager>();
    }
    private void Start()
    {
        _originalPlacePosition = base.PlacePosition;
    }
    protected override void Update()
    {
        base.Update();
        if (base.PlacedItem != null)
        {
            if(base.PlacedItem.TryGetComponent<BasketObject>(out PlacedBasket))
            {
                PlacedBasket.MyFryMachine = this;
            }
        }
        else
        {
            PlacedBasket = null;
        }

        if (IsBasketPlaced && PlacedBasket.MyFryMachine == this)
        {
            if (PlacedBasket != null)
            {

                if (PlacedBasket.fireObject._isOnFire)
                {
                    if (!fireObject._isOnFire)
                    {
                        fireObject.RequestMakeFire();
                        SoundManager.PlayAudio("FireSound", true);
                    }
                    else if (isFireExtinguished)
                    {
                        igniteCoroutine = StartCoroutine(IgniteNearbyTables());
                    }
                }
                else
                {
                    if (igniteCoroutine != null)
                    {
                        StopCoroutine(igniteCoroutine);
                        igniteCoroutine = null;
                        isFireExtinguished = true;
                    }
                }
            }
            else
            {

                if (fireObject._isOnFire)
                {
                    fireObject.RequestExtinguish();
                    isFireExtinguished = true;
                    if (igniteCoroutine != null)
                    {
                        StopCoroutine(igniteCoroutine);
                        igniteCoroutine = null;
                    }
                }
            }


        }

    }

    public IEnumerator IgniteNearbyTables()
    {
        isFireExtinguished = false;
        yield return new WaitForSeconds(5f);

        foreach (var table in NearbyTables)
        {
            if (!table.IsOnFire)
            {
                table.Ignite();
            }
        }
        // 코루틴이 끝나면 초기화
        igniteCoroutine = null;
        isFireExtinguished = true;
    }
}


