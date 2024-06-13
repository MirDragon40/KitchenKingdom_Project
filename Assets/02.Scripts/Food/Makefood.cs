using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Makefood : MonoBehaviourPun
{
    public FoodType FoodType;
    public Transform spawnPoint;

    private Character _nearbyCharacter;
    private bool isPlayerNearby => _nearbyCharacter != null;
    private PhotonView _pv;

    public IHoldable _placedItem;
    public bool HavePlacedItem => _placedItem != null;

    private Transform _handTransform;

    // 박스 열리는 애니메이션
    public Animator _animator;

    private float _checkRange = 1f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!isPlayerNearby)
        {
            return;
        }

        if (_nearbyCharacter.HoldAbility.IsHolding)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _nearbyCharacter.PhotonView.IsMine && !_nearbyCharacter.HoldAbility.IsHolding)
        {
            if (!HavePlacedItem)
            {
                spawnPoint = _nearbyCharacter.HoldAbility.HandTransform;
                if (!IsNearbyHoldable())
                {
                    SpawnFood(FoodType.ToString(), spawnPoint.position, spawnPoint.rotation);
                   //_pv.RPC(nameof(SpawnFood), RpcTarget.All, FoodType.ToString(), spawnPoint.position, spawnPoint.rotation);
                    _nearbyCharacter.GetComponent<Animator>().SetBool("Carry", true);
                    StartCoroutine(BoxOpenAnimation());
                }
            }
        }

        if (!_nearbyCharacter.HoldAbility.IsHolding && Input.GetKeyDown(KeyCode.Space))
        {
            _nearbyCharacter = null;
        }
    }

    private bool IsNearbyHoldable()
    {
        if (!_nearbyCharacter.PhotonView.IsMine)
        {
            return false;
        }
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, _checkRange);
        foreach (Collider collider in colliders)
        {
            IHoldable holdable = collider.GetComponent<IHoldable>();
            if (holdable != null)
            {
                return true;
            }
        }
        return false;
    }

    [PunRPC]
    public void SpawnFood(string foodName, Vector3 position, Quaternion rotation)
    {
        // 음식 생성
        GameObject foodPrefab = Resources.Load<GameObject>(foodName);

        if (foodPrefab != null)
        {
            GameObject food = PhotonNetwork.Instantiate(foodPrefab.name, position, rotation);

            // 음식 오브젝트를 손에 들도록 설정
            IHoldable holdable = food.GetComponent<IHoldable>();
            if (holdable != null)
            {
                holdable.Hold(_nearbyCharacter, _handTransform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlayerNearby)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            _nearbyCharacter = other.GetComponent<Character>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var a = other.GetComponent<Character>();
            if (a == _nearbyCharacter)
            {
                _nearbyCharacter = null;
            }
        }
    }

    private IEnumerator BoxOpenAnimation()
    {
        yield return new WaitForSeconds(1f);
    }
}
