using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stove : CookStand
{
    public PanObject PlacedPan = null;
    private Transform _originalPlacePosition;
    public FireObject fireObject;
    public bool IsPanPlaced => PlacedPan != null;

    public Table[] NearbyTables;

    private void Start()
    {
        _originalPlacePosition = base.PlacePosition;
        fireObject = GetComponent<FireObject>();

    }

    protected override void Update()
    {
        base.Update();
        if (base.PlacedItem != null)
        {
            base.PlacedItem.TryGetComponent<PanObject>(out PlacedPan);
            if (PlacedPan != null && PlacedPan.fireObject != null && PlacedPan.fireObject._isOnFire)
            {
                // 팬이 불에 노출되고 불이 켜져 있으면 스토브의 불도 켭니다.
                if (!fireObject._isOnFire)
                {
                    fireObject.MakeFire();
                }
            }
            else
            {
                // 팬이 불에 노출되지 않거나 불이 꺼져 있으면 스토브의 불도 끕니다.
                if (fireObject._isOnFire)
                {
                    fireObject.Extinguish();
                }
            }

        }


    }
}