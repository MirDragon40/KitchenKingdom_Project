using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMoveAbility : CharacterAbility
{
    private float _gravity = -9.8f;   // 중력 변수
    private float _yVelocity = 0f;

    public float MoveSpeed;
    public float DashSpeed;
    public float RotationSpeed;
    public float DashDuration;
    private float _dirMagnitude;
    private PhotonView _pv;

    private CharacterController _characterController;
    private Animator _animator;
    public SoundManager soundManager;

    public ParticleSystem PowderEffect;
    public ParticleSystem PowderEffect_Dash;

    private bool isDashing = false;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _pv = GetComponent<PhotonView>();
        soundManager = FindObjectOfType<SoundManager>();

        MoveSpeed = 5f;
        DashSpeed = 7f;
        RotationSpeed = 700;
        DashDuration = 0.2f;
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine && PhotonNetwork.IsConnected)
        {
            // SynchronizeAnimation();
            return;
        }
        // 사용자 키보드 입력
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        if (_characterController.isGrounded)
        {
            _yVelocity = 0f; // 땅에 있을 때 y 속도를 초기화
        }
        else
        {
            _yVelocity += _gravity * Time.deltaTime; // 중력 적용
        }

        Vector3 move = dir * MoveSpeed * Time.deltaTime;
        move.y = _yVelocity * Time.deltaTime; // 중력으로 인한 y축 이동 추가

        _characterController.Move(move);

       
        
        _dirMagnitude = dir.magnitude;
        _animator.SetFloat("Move", _dirMagnitude);


        // 파티클 시스템 제어
        if (_dirMagnitude > 0)
        {
            if (!PowderEffect.isPlaying)
            {
                _pv.RPC("WalkEffectPlay", RpcTarget.All);
            }

            if (!soundManager.IsPlaying("Walk"))
            {
                soundManager.PlayAudio("Walk", true);
            }
        }
        else
        {
            if (PowderEffect.isPlaying)
            {
                _pv.RPC("WalkEffectStop", RpcTarget.All);
            }

            if (soundManager.IsPlaying("Walk"))
            {
                soundManager.StopAudio("Walk");
            }
        }
        // 이동하는 방향을 바라보도록 회전
        if (dir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) && !isDashing)
        {
            DashPlay();
        }
    }
    [PunRPC]
    private void WalkEffectPlay()
    {
        PowderEffect.Play();
    }
    [PunRPC]
    private void WalkEffectStop()
    {
        PowderEffect.Stop();
    }
    private void DashPlay()
    {
        PowderEffect_Dash.Play();
        StartCoroutine(Dash());
        soundManager.PlayAudio("Run", true);
    }
    // 대쉬 코루틴 함수
    private IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + DashDuration)
        {
            Vector3 dashDirection = transform.forward;
            _characterController.Move(dashDirection * (DashSpeed * Time.deltaTime));
            yield return null; // 다음 프레임까지 대기
        }

        isDashing = false;
        PowderEffect_Dash.Stop();
        soundManager.StopAudio("Run");
    }

    /*    void SynchronizeAnimation()
        {
            // 애니메이터 파라미터 동기화
            foreach (var param in _animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Float)
                {
                    _animator.SetFloat(param.name, _animator.GetFloat(param.name));
                }
                else if (param.type == AnimatorControllerParameterType.Bool)
                {
                    _animator.SetBool(param.name, _animator.GetBool(param.name));
                }
            }
        }*/
}
