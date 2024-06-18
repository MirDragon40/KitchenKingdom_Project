using System.Collections;
using UnityEngine;

public class Table : MonoBehaviour
{
    public bool IsOnFire = false;
    public ParticleSystem fireEffect;

    public Table[] NearbyTables;
    public Stove[] NearbyStoves;
    private bool isPowderTouching = false;
    private float powderContactTime = 0;

    private void Start()
    {
        if (fireEffect == null)
        {
            fireEffect = GetComponentInChildren<ParticleSystem>();
        }
    }

    public void Ignite()
    {
        IsOnFire = true;
        fireEffect.Play();
        Debug.Log("이그나이트");

        StartCoroutine(IgniteNearbyTables());
        StartCoroutine(IgniteNearbyStoves());
    }

    public void Extinguish()
    {
        IsOnFire = false;
        fireEffect.Stop();
    }

    IEnumerator IgniteNearbyTables()
    {
        yield return new WaitForSeconds(5f);

        foreach (var table in NearbyTables)
        {
            if (!table.IsOnFire)
            {
                table.Ignite();
            }
        }
    }

    IEnumerator IgniteNearbyStoves()
    {
        yield return new WaitForSeconds(5f);

        foreach (var stove in NearbyStoves)
        {
            if (!stove.fireObject._isOnFire)
            {
                stove.fireObject.MakeFire();
                stove.StartCoroutine(stove.IgniteNearbyTables());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsOnFire && other.CompareTag("Powder"))
        {
            isPowderTouching = true;
            powderContactTime += Time.deltaTime;
            Debug.Log(powderContactTime);
            if (powderContactTime >= 2f)
            {
                Extinguish();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsOnFire && other.CompareTag("Powder"))
        {
            isPowderTouching = false;
            powderContactTime = 0f;
        }
    }
    public void SetOnFire(bool isOnFire)
    {
        IsOnFire = isOnFire;
    }
}