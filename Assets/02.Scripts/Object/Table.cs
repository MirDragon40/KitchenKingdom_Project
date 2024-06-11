using System.Collections;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Table[] NearbyTables;
    public ParticleSystem FireParticle;
 
    public void Ignite()
    {
        StartCoroutine(SpreadFireToNearbyTables());
    }

    private IEnumerator SpreadFireToNearbyTables()
    {
        yield return new WaitForSeconds(5f);

        foreach (var table in NearbyTables)
        {
            if (table != null)
            {
                Debug.Log("ㅇㅇㅇ");
                table.FireParticle.Play();
                table.Ignite();
            }
        }
    }
}