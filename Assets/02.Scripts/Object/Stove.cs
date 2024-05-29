using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : CookStand
{
    public GameObject PlusUI;
    public PanObject PlacedPan = null;
    public bool IsPanPlaced => PlacedPan != null;

    private void Update()
    {

        PlacedPan = PlacePosition.GetComponentInChildren<PanObject>();
        
        if (IsPanPlaced && PlusUI.activeSelf)
        {
            PlusUI.SetActive(false);
        }
        else if (!IsPanPlaced && !PlusUI.activeSelf)
        {
            PlusUI.SetActive(true);
        }
    }

}

