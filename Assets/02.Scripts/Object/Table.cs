using System.Collections;
using UnityEngine;

public class Table : MonoBehaviour
{
    public bool _isOnFire = false;
    public ParticleSystem fireEffect;

    public Table[] NearbyTables;
    private bool isTouchingPowder = false;
    private float powderContactTime = 0f;


    private void Start()
    {
        fireEffect = GetComponentInChildren<ParticleSystem>();
    }

    public void Ignite()
    {
        _isOnFire = true;
        fireEffect.Play();
        Debug.Log("이그나이트");

        StartCoroutine(IgniteNearbyTables());
    }

    public void Extinguish()
    {
        _isOnFire = false;
        fireEffect.Stop();
    }

    IEnumerator IgniteNearbyTables()
    {
        yield return new WaitForSeconds(5f);

        foreach (var table in NearbyTables)
        {
            if (!table._isOnFire)
            {
                table.Ignite();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powder"))
        {
            isTouchingPowder = true;
            powderContactTime = Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Powder"))
        {
            if (isTouchingPowder && Time.deltaTime - powderContactTime >= 2f && _isOnFire)
            {
                Debug.Log("dd");
                Extinguish();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Powder"))
        {
            isTouchingPowder = false;
            powderContactTime = 0f;

        }
    }

}