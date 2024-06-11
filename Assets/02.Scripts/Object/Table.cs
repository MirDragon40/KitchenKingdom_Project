using System.Collections;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Table[] NearbyTables;

    private bool isStoveFireActive = false;


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
                table.Ignite();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Table"))
        {

        }

    }

    public void UpdateStoveFireStatus(bool isFireActive)
    {
        isStoveFireActive = isFireActive;
    }
}