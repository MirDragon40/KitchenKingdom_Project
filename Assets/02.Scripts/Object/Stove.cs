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

    private bool _isTableNotified = false;

    private void Start()
    {
        _originalPlacePosition = base.PlacePosition;
        fireObject = GetComponent<FireObject>();

    }

    protected override void Update()
    {
        base.Update();
        if (PlacedPan != null)
        {
            // 팬의 불 상태를 확인하여 스토브의 불 상태를 업데이트
            if (PlacedPan.fireObject._isOnFire)
            {
                if (!fireObject._isOnFire)
                {
                    fireObject.MakeFire();
                    GetComponent<Table>().Ignite();
                    NotifyTables();
                }

            }
        }

    }
    private void NotifyTables()
    {
        if (!_isTableNotified)
        {
            foreach (Table table in NearbyTables)
            {
                StartCoroutine(DelayedFire(table));
            }
            _isTableNotified = true;
        }
    }
    private IEnumerator DelayedFire(Table table)
    {
        yield return new WaitForSeconds(5f);
        table.Ignite();
    }
}