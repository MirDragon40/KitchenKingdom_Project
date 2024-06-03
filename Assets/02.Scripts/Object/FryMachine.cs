using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryMachine : CookStand
{
    public BasketObject PlacedBasket = null;
    private Transform _originalPlacePosition;

    public bool IsBasketPlaced => PlacedBasket != null;

    private void Start()
    {
        _originalPlacePosition = base.PlacePosition;
    }
    protected override void Update()
    {
        base.Update();
        if (base.PlacedItem != null)
        {
            base.PlacedItem.TryGetComponent<BasketObject>(out PlacedBasket);
        }
        else
        {
            PlacedBasket = null;
        }

    }

}
