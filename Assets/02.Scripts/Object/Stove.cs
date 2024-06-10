using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stove : CookStand
{
    public PanObject PlacedPan = null;
    private Transform _originalPlacePosition;
    public bool IsPanPlaced => PlacedPan != null;

    public ParticleSystem FireParticle;

    private bool fireTriggered = false;
    private bool isPowderTouching = false;
    private float contactTime = 0f;

    public Table[] NearbyTables;

    private void Start()
    {
        _originalPlacePosition = base.PlacePosition;
        FireParticle = GetComponentInChildren<ParticleSystem>();
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

        if (PlacedPan != null && PlacedPan.GrillingIngrediant != null)
        {
            ManageGrillingProgress();
        }
        else
        {
            fireTriggered = false;
        }

        if (!isPowderTouching && contactTime > 0)
        {
            contactTime -= Time.deltaTime;
            if (contactTime < 0)
            {
                contactTime = 0;
            }
        }

        isPowderTouching = false;
    }

    private void ManageGrillingProgress()
    {
        var grillingIngredient = PlacedPan.GrillingIngrediant;
        PlacedPan.GrillingSlider.gameObject.SetActive(true);
        grillingIngredient.StartGrilling();
        PlacedPan.GrillingSlider.value = grillingIngredient.CookProgress;

        if (grillingIngredient.CookProgress >= 2f && grillingIngredient.CookProgress < 2.9f)
        {
            PlacedPan.dangerIndicator.ShowDangerIndicator(PlacedPan.dangerSprite);
        }
        else
        {
            PlacedPan.dangerIndicator.HideDangerIndicator();
        }

        if (grillingIngredient.CookProgress >= 3f && !fireTriggered)
        {
            PlacedPan.fireObject.MakeFire();
            PlacedPan.FireSlider.gameObject.SetActive(true);
            Ignite();
            fireTriggered = true;
        }
    }

    public void Ignite()
    {
        FireParticle.Play();
        StartCoroutine(SpreadFireToNearbyTables());
    }

    private IEnumerator SpreadFireToNearbyTables()
    {
        yield return new WaitForSeconds(5f);

        foreach (var table in NearbyTables)
        {
            if (table != null)
            {
                table.Ignite();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (fireTriggered && other.CompareTag("Powder"))
        {
            isPowderTouching = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (fireTriggered && other.CompareTag("Powder"))
        {
            isPowderTouching = true;
            contactTime += Time.deltaTime;
            if (contactTime >= 2f)
            {
                Extinguish();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Powder"))
        {
            isPowderTouching = false;
        }
    }

    public void Extinguish()
    {
        FireParticle.Stop();
        fireTriggered = false;
        contactTime = 0;
        StopCoroutine(SpreadFireToNearbyTables());
    }
}