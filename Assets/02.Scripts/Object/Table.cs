using UnityEngine;

public class Table : MonoBehaviour
{
    public ParticleSystem FireParticle;

    public void Start()
    {
        FireParticle = GetComponentInChildren<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Table"))
        {
            FireParticle.Play();
        }

    }
}