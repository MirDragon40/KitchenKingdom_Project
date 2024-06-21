using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Extinguisher : IHoldable
{
    private ParticleSystem _powderEffect;
    public BoxCollider _boxCollider;
    public bool isPress = false;
    private PhotonView _pv;
    public Transform StartPosition;
    public SoundManager soundManager;
    private bool isShooting = false; // 소화기 분말 연사중
    private Coroutine _currentShotCoroutine;

    public override Vector3 DropOffset => new Vector3(0.3f, 0, 0);
    private void Awake()
    {
        _powderEffect = GetComponentInChildren<ParticleSystem>();
        _boxCollider.enabled = false;
        _pv = GetComponent<PhotonView>();
        soundManager = FindObjectOfType<SoundManager>();
    }
    private void Start()
    {
        if (StartPosition != null)
        {
            Place(StartPosition);
        }
    }
    public override void Hold(Character character, Transform handTransform)
    {

        if (_pv.OwnerActorNr != character.PhotonView.OwnerActorNr)
        {
            _pv.TransferOwnership(character.PhotonView.OwnerActorNr);
        }

        _holdCharacter = character;

        // 각 아이템이 잡혔을 때 해줄 초기화 로직
        transform.SetParent(handTransform);
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.Euler(0, 90, 0);

    }

    [PunRPC]
    public void Shot(bool state)
    {
        isPress = state;
        if (state)
        {
            _powderEffect.Play();
            _boxCollider.enabled = true;
            SoundManager.Instance.PlayAudio("Powder",true);
        }
        else
        {
            _powderEffect.Stop();
            _boxCollider.enabled = false;
            SoundManager.Instance.StopAudio("Powder");
        }

        if (_currentShotCoroutine != null)
        {
            StopCoroutine(_currentShotCoroutine);
        }

        if (!state)
        {
            _currentShotCoroutine = StartCoroutine(StopSoundAfterDelay(0.5f));
        }
    }

    private IEnumerator StopSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        soundManager.StopAudio("Powder");
    }

    private void Update()
    {
        if (IsHold && _holdCharacter.PhotonView.IsMine) // 소화기를 들고 있는 사람만 소화기를 작동시킬 수 있음
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Shot(true);
                _pv.RPC("Shot", RpcTarget.All, true); 
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Shot(false);
                _pv.RPC("Shot", RpcTarget.All, false); 
            }
        }
    }

    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {

        // 저장한 위치와 회전으로 소화기 배치
        transform.position = dropPosition;
        Quaternion additionalRotation = Quaternion.Euler(0, -90, 0);
        Quaternion finalRotation = dropRotation * additionalRotation;

        _powderEffect.Stop();
        SoundManager.Instance.StopAudio("Powder");
        transform.parent = null;
        // 각 아이템이 놓여질 때 해줄 초기화 로직


        _holdCharacter = null;

    }

    public override void Place(Transform place)
    {
        transform.parent = place;
        transform.position = place.position;
        Quaternion additionalRotation = Quaternion.Euler(0, -90, 0);


        _powderEffect.Stop();
        SoundManager.Instance.StopAudio("Powder");
        _holdCharacter = null;

    }

    /*    public void A(Collider other)
        {
            if (!isPress)
                return;

            if (other.CompareTag("Fire"))
            {
                Debug.Log(other.gameObject.name);
                var fireEffect = other.GetComponent<ParticleSystem>();
                if (fireEffect != null)
                {
                    fireEffect.Stop();
                }
            }
        }*/
}