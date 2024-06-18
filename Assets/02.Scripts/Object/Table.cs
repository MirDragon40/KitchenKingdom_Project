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

    private Coroutine igniteNearbyTablesCoroutine;
    private Coroutine igniteNearbyStovesCoroutine;
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
        if (fireEffect != null)
        {
            fireEffect.Play(); // 파티클 재생
        }
        Debug.Log("이그나이트");

        igniteNearbyTablesCoroutine = StartCoroutine(IgniteNearbyTables());
        igniteNearbyStovesCoroutine = StartCoroutine(IgniteNearbyStoves());
    }

    public void Extinguish()
    {
        IsOnFire = false;
        if (fireEffect != null)
        {
            fireEffect.Stop(); // 파티클 정지
        }

        if (igniteNearbyTablesCoroutine != null)
        {
            StopCoroutine(igniteNearbyTablesCoroutine);
            igniteNearbyTablesCoroutine = null;
        }

        if (igniteNearbyStovesCoroutine != null)
        {
            StopCoroutine(igniteNearbyStovesCoroutine);
            igniteNearbyStovesCoroutine = null;
        }
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
            if (stove != null && !stove.IsOnFire)
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
}