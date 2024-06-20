using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Table : MonoBehaviourPun
{
    public bool IsOnFire = false;
    public ParticleSystem fireEffect;
    public SoundManager soundManager;
    private PhotonView _pv;


    public Table[] NearbyTables;
    public Stove[] NearbyStoves;
    private bool isPowderTouching = false;
    private float powderContactTime = 0;

    private Coroutine igniteNearbyTablesCoroutine;
    private Coroutine igniteNearbyStovesCoroutine;

    private bool anyFireIsOn = false;   // 모든 불이 꺼졌는지


    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        if (fireEffect == null)
        {
            fireEffect = GetComponentInChildren<ParticleSystem>();
        }
        soundManager = FindObjectOfType<SoundManager>();
    }
    public void RequestIgnite()
    {
        if (IsOnFire)
        {
            return;
        }
        if (PhotonNetwork.IsMasterClient)
        { 
            _pv.RPC("Ignite", RpcTarget.All);
        }
    }
    [PunRPC]
    public void Ignite()
    {
        if (!IsOnFire) // 이미 불이 붙어있는 경우 다시 붙지 않도록 체크
        {
            IsOnFire = true;
            if (fireEffect != null)
            {
                fireEffect.Play(); // 파티클 재생
            }
            Debug.Log("이그나이트");
            soundManager.PlayAudio("Fire", true);

            if (igniteNearbyTablesCoroutine != null)
            {
                StopCoroutine(igniteNearbyTablesCoroutine);
            }
            igniteNearbyTablesCoroutine = StartCoroutine(IgniteNearbyTables());

            if (igniteNearbyStovesCoroutine != null)
            {
                StopCoroutine(igniteNearbyStovesCoroutine);
            }
            igniteNearbyStovesCoroutine = StartCoroutine(IgniteNearbyStoves());
        }
    }
    public void RequestExtinguish()
    {
        if (!IsOnFire)
        {
            return;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            _pv.RPC("ExtinguishTable", RpcTarget.All);
        }
    }
    [PunRPC]
    public void ExtinguishTable()
    {
        if (!IsOnFire)
        {
            return;
        }
        IsOnFire = false;
        if (fireEffect != null)
        {
            fireEffect.Stop(); // 파티클 정지
        }

        // 모든 불이 꺼졌는지 확인
        anyFireIsOn = false;
        foreach (var table in NearbyTables)
        {
            if (table != null && table.IsOnFire)
            {
                anyFireIsOn = true;
                break;
            }
        }
        foreach (var stove in NearbyStoves)
        {
            if (stove != null && stove.IsOnFire)
            {
                anyFireIsOn = true;
                break;
            }
        }

        // 모든 불이 꺼졌다면 사운드 정지
        if (!anyFireIsOn)
        {
            soundManager.StopAudio("Fire");
        }

        // 불이 꺼지면 코루틴을 중지하고 null로 설정
        if (igniteNearbyTablesCoroutine != null)
        {
            StopCoroutine(igniteNearbyTablesCoroutine);
            igniteNearbyTablesCoroutine = null;
        }

        if (igniteNearbyStovesCoroutine != null)
        {
            StopCoroutine(igniteNearbyStovesCoroutine);
            igniteNearbyStovesCoroutine = null;
        }
    }

    IEnumerator IgniteNearbyTables()
    {
        yield return new WaitForSeconds(8f);

        // 불이 붙는 과정 중 불이 꺼지면 종료
        if (!IsOnFire)
        {
            yield break;
        }

        foreach (var table in NearbyTables)
        {
            if (table != null && !table.IsOnFire)
            {
                table.RequestIgnite();
            }
        }
    }
        
    IEnumerator IgniteNearbyStoves()
    {
        yield return new WaitForSeconds(8f);

        // 불이 붙는 과정 중 불이 꺼지면 종료
        if (!IsOnFire)
        {
            yield break;
        }

        foreach (var stove in NearbyStoves)
        {
            if (stove != null && !stove.IsOnFire)
            {
                stove.fireObject.RequestMakeFire();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (PhotonNetwork.IsMasterClient && other.CompareTag("Powder"))
        {
            isPowderTouching = true;
            powderContactTime += Time.deltaTime;
            Debug.Log(powderContactTime);
            if (powderContactTime >= 2f)
            {
                RequestExtinguish();

                if (igniteNearbyTablesCoroutine != null)
                {
                    StopCoroutine(igniteNearbyTablesCoroutine);
                    igniteNearbyTablesCoroutine = null;
                }

                if (igniteNearbyStovesCoroutine != null)
                {
                    StopCoroutine(igniteNearbyStovesCoroutine);
                    igniteNearbyStovesCoroutine = null;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsOnFire && other.CompareTag("Powder"))
        {
            isPowderTouching = false;
            powderContactTime = 0f;
        }
    }
}