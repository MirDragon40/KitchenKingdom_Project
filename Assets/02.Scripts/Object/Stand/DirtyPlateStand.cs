using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlateStand : MonoBehaviour
{

    public List<GameObject> DirtyPlates;
    public int DirtyPlateNum = 5;

    private CharacterHoldAbility characterHoldAbility;


    private void Awake()
    {
        foreach (GameObject plate in DirtyPlates)
        {
            plate.SetActive(false);
        }
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterHoldAbility = other.GetComponent<CharacterHoldAbility>();
        }
    }

    private void GiveDirtyPlates()
    {
        
    }
}
