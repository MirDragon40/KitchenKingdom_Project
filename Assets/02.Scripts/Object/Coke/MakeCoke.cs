using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCoke : MonoBehaviour
{
    public GameObject CokePrefab;

    public Transform CokeSpawnPoint;

    private bool _isPlayerAround;
    public SoundManager soundManager;
    private bool isCokeSpawned = false;

    public void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }
    private void Update()
    {
        if (_isPlayerAround && Input.GetKeyDown(KeyCode.Space)) 
        {
            if(CokeSpawnPoint.childCount == 0) 
            {
                SpawnCoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Character>().PhotonView.IsMine) 
        {
            _isPlayerAround = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Character>().PhotonView.IsMine) 
        {
            _isPlayerAround = false;
        }
    }

    private void SpawnCoke() 
    {
        GameObject newCoke = PhotonNetwork.Instantiate("Coke", CokeSpawnPoint.position, Quaternion.identity);
        newCoke.transform.parent = CokeSpawnPoint;
        soundManager.PlayAudio("Cola", true);
        isCokeSpawned = true;
        StartCoroutine(Coroutine_CokeSoundStop());
    }
    public IEnumerator Coroutine_CokeSoundStop()
    {
        yield return new WaitForSeconds(3.5f);
        soundManager.StopAudio("Cola");
    }

}
