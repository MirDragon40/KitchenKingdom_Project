using System.Collections;
using UnityEngine;

public class FireObject : MonoBehaviour
{
    public ParticleSystem FireEffect;
    private bool _isonFire = false;
    private void Awake()
    {
       FireEffect = GetComponentInChildren<ParticleSystem>();
    }

    public void MakeFire()
    {
        if (_isonFire)
        {
            return;
        }
        FireEffect.Play();
        _isonFire = true;
        Debug.Log("불이야");
    }

    IEnumerator FireStop_Coroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Extinguish();
    }

    public void Extinguish()
    {
        FireEffect.Stop();
        Debug.Log("STOP");
    }

    // 분말 파티클과 불이 닿았을 때 처리
    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Powder"))
        {
            StartCoroutine(FireStop_Coroutine(10f));
        }
    }

}