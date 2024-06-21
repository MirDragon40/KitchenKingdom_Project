using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCoke : MonoBehaviourPun
{
    public GameObject CokePrefab;

    public Transform CokeSpawnPoint;

    private bool _isPlayerAround;
    public SoundManager soundManager;
    private bool isCokeSpawned = false;
    private bool _isPickUpable = false;
    private PhotonView _pv;
    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
    }

    public void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }
    private void Update()
    {
        if (_isPlayerAround && Input.GetKeyDown(KeyCode.LeftControl)) 
        {
            if(CokeSpawnPoint.childCount == 0) 
            {
                _pv.RPC("SpawnCoke", RpcTarget.All);
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
    [PunRPC]
    private void SpawnCoke()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject newCoke = PhotonNetwork.InstantiateRoomObject("Coke", CokeSpawnPoint.position, Quaternion.identity);
            newCoke.transform.SetParent(CokeSpawnPoint);
        }
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
