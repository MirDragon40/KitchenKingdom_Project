using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stove : CookStand
{
    public GameObject PlusUI;
    public PanObject PlacedPan = null;
    private Transform _originalPlacePosition;
    public bool IsPanPlaced => PlacedPan != null;

    private void Start()
    {
        _originalPlacePosition = base.PlacePosition;
    }
    protected override void Update()
    {
        base.Update();
        if (base.PlacedItem != null)
        {
            base.PlacedItem.TryGetComponent<PanObject>(out PlacedPan);
        }
        else
        {
            PlacedPan = null;
        }
        
        
        
        if (IsPanPlaced && PlusUI.activeInHierarchy)
        {
            PlusUI.SetActive(false);
        }
        else if (!IsPanPlaced && !PlusUI.activeInHierarchy)
        {
            PlusUI.SetActive(true);
        }
    }

}

