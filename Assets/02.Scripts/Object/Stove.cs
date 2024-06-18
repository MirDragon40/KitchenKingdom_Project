using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stove : CookStand
{
    public PanObject PlacedPan = null;
    public Transform _originalPlacePosition;
    public FireObject fireObject;
    public bool IsPanPlaced => PlacedPan != null;
   
    public Table[] NearbyTables;
    private Coroutine igniteCoroutine;
    private bool isFireExtinguished = true;

    public bool IsOnFire => fireObject != null && fireObject._isOnFire;
    private void Start()
    {
        _originalPlacePosition = base.PlacePosition;
        fireObject = GetComponent<FireObject>();
    }

    protected override void Update()
    {
        base.Update();
        if (PlacedItem != null)
        {
            if (PlacedItem.TryGetComponent<PanObject>(out PlacedPan))
            {
                if (PlacedPan.MyStove == null)
                {
                    PlacedPan.MyStove = this;
                }
            }
            else
            {
                PlacedPan = null;
            }
        }
        if (IsPanPlaced && PlacedPan.MyStove == this)
        {
            if (PlacedPan != null)
            {

                if (PlacedPan.fireObject._isOnFire)
                {
                    if (!fireObject._isOnFire)
                    {
                        fireObject.MakeFire();

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
                    fireObject.Extinguish();
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
    }

}