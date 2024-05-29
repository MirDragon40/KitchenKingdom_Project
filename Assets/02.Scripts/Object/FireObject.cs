using System.Collections;
using UnityEngine;

public class FireObject : MonoBehaviour
{
    public ParticleSystem fireEffect;
    public ParticleSystem powderEffect; // 분말 파티클 추가

    private void Awake()
    {
        fireEffect = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartFire();
        }
    }

    public void StartFire()
    {
        StartCoroutine(StartFireCoroutine(1f)); // 1초 후에 화재 발생
    }

    private IEnumerator StartFireCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        fireEffect.Play();
        // 추가적인 화재 발생 로직을 여기에 작성
    }

    public void Extinguish()
    {
        fireEffect.Stop();
        Debug.Log("STOP");
    }

    // 분말 파티클과 불이 닿았을 때 처리
    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Powder"))
        {
            // 불 파티클을 중지시킴
            fireEffect.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Extinguisher"))
        {
            Extinguish();
            Debug.Log("00");
        }
    }
}