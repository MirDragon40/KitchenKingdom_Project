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
    private bool isSoundPlaying = false; // 불이 켜져있는 팬이 있는지 체크
    public FireObject[] Tables;
    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        soundManager = FindObjectOfType<SoundManager>();
        Tables = FindObjectsOfType<FireObject>();
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
        if (!isSoundPlaying)
        {
            soundManager.PlayAudio("Fire", true, true);
            isSoundPlaying = true; 
        }
    }
    public void RequestExtinguish()
    {
        Debug.Log("불꺼짐");
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

        if (!IsAnyFanOnFire())
        {
            soundManager.StopAudio("Warning");
            soundManager.StopAudio("Fire");
            isSoundPlaying = false; // 사운드가 중지됨을 표시
        }


        contactTime = 0f;
    }
    private bool IsAnyFanOnFire()
    {
        foreach (var Table in Tables)
        {
            if (Table._isOnFire)
            {
                return true; // 하나라도 불이 켜져 있으면 true 반환
            }
        }
        return false; // 모든 테이블이 불이 꺼져 있으면 false 반환
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