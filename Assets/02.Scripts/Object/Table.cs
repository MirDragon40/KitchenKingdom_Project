using System.Collections;
using UnityEngine;

public class Table : MonoBehaviour
{
    public ParticleSystem FireParticle;
    public Table[] NearbyTables;

    public void Start()
    {
        FireParticle = GetComponentInChildren<ParticleSystem>();
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
        Debug.Log(other);
        if (other.CompareTag("Table"))
        {
            FireParticle.Play();
        }

    }
}