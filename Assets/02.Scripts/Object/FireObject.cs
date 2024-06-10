using System.Collections;
using UnityEngine;

public class FireObject : MonoBehaviour
{
    public PanObject panObject;

    public ParticleSystem FireEffect;
    public bool _isOnFire = false;   // 불이 붙어있는 상태인지 나타냄
    public float contactTime = 0f;   // 'Powder'와의 접촉 시간을 측정
    public bool isFireActive = true; // 불이 활성화 상태인지 나타냄
    private void Awake()
    {
       FireEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        // 불이 이미 꺼졌다면 추가적인 처리를 하지 않음
        if (!isFireActive) return;
    }

    public void MakeFire()
    {
        if (_isOnFire)
        {
            return;
        }
        FireEffect.Play();
        _isOnFire = true;
        isFireActive = true;
        Debug.Log("불이야");
    }

    public void Extinguish()
    {
        isFireActive = false; // 불이 꺼졌다고 상태 변경
        contactTime = 0f;
        _isOnFire = false;

        FireEffect.Stop(); // 불 효과를 비활성화

        isFireActive = true;

    }

}