using Photon.Pun;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
public class FireObject : MonoBehaviourPun
{
    public bool _isOnFire = false;
    public ParticleSystem fireEffect;
    private bool isPowderTouching = false;
    public float contactTime = 0;
    private PhotonView _pv;

    public SoundManager soundManager;
    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        soundManager = FindObjectOfType<SoundManager>();
    }
    public void RequestMakeFire()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _pv.RPC("MakeFire", RpcTarget.All);
        }

    }
    [PunRPC]
    public void MakeFire()
    {
        Debug.Log(gameObject.name);
        _isOnFire = true;
        fireEffect.Play();
    }
    public void RequestExtinguish()
    {

            _pv.RPC("ExtinguishFireObject", RpcTarget.All);
        
    }
    [PunRPC]
    public void ExtinguishFireObject()
    {
        _isOnFire = false;
        if (fireEffect != null)
        {
            fireEffect.Stop();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isOnFire && other.CompareTag("Powder"))
        {
            isPowderTouching = true;
            contactTime += Time.deltaTime;
            if (contactTime >= 2f)
            {
                RequestExtinguish();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isOnFire && other.CompareTag("Powder"))
        {
            isPowderTouching = false;
            contactTime = 0f;
        }
    }
}