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
        _pv.RPC("MakeFire", RpcTarget.All);

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
        _pv.RPC("Extinguish", RpcTarget.All);
    }
    [PunRPC]
    public void Extinguish()
    {
        _isOnFire = false;
        fireEffect.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isOnFire && other.CompareTag("Powder"))
        {
            isPowderTouching = true;
            contactTime += Time.deltaTime;
            if (contactTime >= 2f)
            {
                Extinguish();
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