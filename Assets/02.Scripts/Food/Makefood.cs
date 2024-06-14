using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
                    //SpawnFood(FoodType.ToString(), spawnPoint.position, spawnPoint.rotation);
                    // 룸 오브젝트 생성은 방장만이 할 수 있으므로 -> 방장에게만 요청한다.
                   _pv.RPC(nameof(RequestSpawnFood), RpcTarget.MasterClient, FoodType.ToString(), spawnPoint.position, spawnPoint.rotation);
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
    public void RequestSpawnFood(string foodName, Vector3 position, Quaternion rotation)
    {
        if(PhotonNetwork.IsMasterClient == false)
        {
            Debug.Log("방장이 아닌데 RequestSpawnFood를 호출하려고 한다..");
            return;
        }

        // 음식 생성
        GameObject foodPrefab = Resources.Load<GameObject>(foodName);

        if (foodPrefab != null)
        {
            // 룸에 종속되는 룸 오브젝트 생성 <- '방장'만 할 수 있다.
            //GameObject food = PhotonNetwork.InstantiateRoomObject(foodPrefab.name, position, rotation);
            GameObject food = PhotonNetwork.InstantiateRoomObject(foodName, position, rotation);
            // 방장이 룸 오브젝트를 생성하면 자동으로 나머지 컴퓨터에서도 생성된다 -> 동기화


            // 음식 오브젝트를 손에 들도록 설정
            // Problem: 그러나 이 부분은 방장 컴퓨터에서만 실행된다.
            IHoldable holdable = food.GetComponent<IHoldable>();
            if (holdable != null)
            {
                // 음식 오브젝트.잡다(잡을 캐릭터, 손 위치)


                //holdable.Hold(_nearbyCharacter, _nearbyCharacter.GetComponent<CharacterHoldAbility>().HandTransform);

                // Answer: RPC를 이용해 모든 컴퓨터들에게 실행해준다.
                // RPC에서 허용 가능한 매개변수 자료형 -> 숫자, 문자, 벡터, 쿼터니언이므로 viewID를 넘겨준다.
                _pv.RPC(nameof(ResponseHold), RpcTarget.AllBuffered, food.GetComponent<PhotonView>().ViewID, _nearbyCharacter.PhotonView.ViewID);
            }
        }
    }

    [PunRPC]

    public void ResponseHold(int foodPhotonViewID, int characterPhtonViewID)
    {
        PhotonView foodPV      = PhotonView.Find(foodPhotonViewID);
        PhotonView characterPV = PhotonView.Find(characterPhtonViewID);

        if(foodPV == null || characterPV == null)
        {
         
            return;
        }

        IHoldable holdable  = foodPV.GetComponent<IHoldable>();
        Character character = characterPV.GetComponent<Character>();

        holdable.Hold(character, character.HoldAbility.HandTransform);
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
