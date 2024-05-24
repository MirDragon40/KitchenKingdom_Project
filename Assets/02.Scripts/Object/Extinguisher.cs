using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : IHoldable
{
    private ParticleSystem _powderEffect;

    private void Awake()
    {
        _powderEffect = GetComponentInChildren<ParticleSystem>();
    }
    public override void Hold(Character character, Transform handTransform)
    {
        _holdCharacter = character;
        Debug.Log(_holdCharacter.name);

        // 각 아이템이 잡혔을 때 해줄 초기화 로직
        transform.parent = handTransform;

         transform.localPosition = new Vector3(0.52f, 0f, -0.1f);
        transform.localRotation = Quaternion.Euler(0, 90, 0);

    }

    public void Shot()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Shot");
           _powderEffect.Play();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _powderEffect.Stop();
        }

    }    


    private void Update()
    {

        if(IsHold)
        {
            Shot();
        }
        
    }

    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {

        // 저장한 위치와 회전으로 음식 배치
        transform.position = dropPosition;
        transform.rotation = dropRotation;


        transform.parent = null;
        // 각 아이템이 ㄴ 때 해줄 초기화 로직


        _holdCharacter = null;

    }
}