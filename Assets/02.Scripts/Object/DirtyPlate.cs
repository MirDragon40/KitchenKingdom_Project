using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlate : MonoBehaviour
{
    public List<GameObject> DirtyPlates;
    public int DirtyPlateNum = 5;


    private void Awake()
    {
        foreach (GameObject plate in DirtyPlates)
        {
            plate.SetActive(false);
        }
    }


    private void Update()
    {
        UpdatePlates();
    }


    private void UpdatePlates()
    {
        // DirtyPlates 업데이트
        for (int i = 0; i < DirtyPlates.Count; i++)
        {
            if (i < DirtyPlateNum)
            {
                DirtyPlates[i].SetActive(true);
            }
            else
            {
                DirtyPlates[i].SetActive(false);
            }
        }

    }

   
}
