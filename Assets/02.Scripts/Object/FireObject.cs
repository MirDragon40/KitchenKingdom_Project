using System.Collections;
using UnityEngine;

public class FireObject : MonoBehaviour
{
    public bool _isOnFire = false;
    public ParticleSystem fireEffect;
    private bool isPowderTouching = false;
    public float contactTime = 0;

    public SoundManager soundManager;
    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void MakeFire()
    {
        Debug.Log(gameObject.name);
        _isOnFire = true;
        fireEffect.Play();
    }

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