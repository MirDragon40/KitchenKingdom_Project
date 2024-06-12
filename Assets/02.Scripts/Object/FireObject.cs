using System.Collections;
using UnityEngine;

public class FireObject : MonoBehaviour
{
    public PanObject panObject;

    public ParticleSystem FireEffect;
    public bool _isOnFire = false;   // 불이 붙어있는 상태인지 나타냄
    public float contactTime = 0f;   // 'Powder'와의 접촉 시간을 측정
    
    private void Awake()
    {
       FireEffect = GetComponentInChildren<ParticleSystem>();
    }

    public void MakeFire()
    {
        if (_isOnFire)
        {
            return;
        }
        FireEffect.Play();
        _isOnFire = true;
        Debug.Log("불이야");
    }

    public void Extinguish()
    {
        FireEffect.Stop(); 
        contactTime = 0f;
        _isOnFire = false;
        Debug.Log("불꺼짐");

    }

}