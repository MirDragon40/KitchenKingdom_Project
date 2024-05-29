using System.Collections;
using UnityEngine;

public class FireObject : MonoBehaviour
{
    public ParticleSystem _fireEffect;
    public Collider extinguisherCollider;

    private void Awake()
    {
        _fireEffect = GetComponentInChildren<ParticleSystem>();
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
        _fireEffect.Play();
        // 추가적인 화재 발생 로직을 여기에 작성
    }

    public void Extinguish()
    {
        _fireEffect.Stop();
        Debug.Log("STOP");
    }

   
    void OnParticleCollision(GameObject other)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == extinguisherCollider.gameObject)
        {
            Extinguish();
            Debug.Log(00);
        }
    }
}