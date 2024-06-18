using Photon.Pun;
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class DirtyPlateStand : MonoBehaviourPun 
{
    public List<GameObject> DirtyPlates; 
    public int DirtyPlateNum;

    private CharacterHoldAbility characterHoldAbility;
    private Character _nearbyCharacter;

    private DirtyPlate dirtyPlate;

    private bool isPlayerInTrigger = false; 
    private bool isPlayerHoldingDirtyPlate = false;


    // 플레이어가 근처에 있는지 여부
    private bool isPlayerNearby => _nearbyCharacter != null;
    

    private PhotonView _pv;

    private void Awake()
    {
        foreach (GameObject plate in DirtyPlates) // 더러운 접시 리스트의 모든 접시를 비활성화
        {
            plate.SetActive(false);
        }

        _pv = GetComponent<PhotonView>();
    }

    private void Start() 
    {
        // 처음 시작할때의 접시 개수 설정
        DirtyPlateNum = 0;

        UpdatePlates(); // 접시 상태 업데이트
    }

    private void Update()
    {
        if (!isPlayerNearby) 
        {
            return;
        }

        // 플레이어가 트리거 안에 있고 스페이스 키를 눌렀으며 자신의 캐릭터일 때
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space) && _nearbyCharacter.PhotonView.IsMine)
        {
            GiveDirtyPlates(); // 더러운 접시를 줌
        }
    }

    private void UpdatePlates() // 접시 상태를 업데이트하는 메서드
    {
        for (int i = 0; i < DirtyPlates.Count; i++) // 더러운 접시 리스트를 순회하며
        {
            DirtyPlates[i].SetActive(i < DirtyPlateNum); // DirtyPlateNum 이하의 인덱스의 접시만 활성화
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
            characterHoldAbility = other.GetComponent<CharacterHoldAbility>(); 
            _nearbyCharacter = other.GetComponent<Character>();
            isPlayerInTrigger = true; 

            UpdateDirtyPlateStatus(); // 더러운 접시 상태 업데이트
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && characterHoldAbility != null)
        {
            UpdateDirtyPlateStatus(); // 더러운 접시 상태 업데이트
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            isPlayerHoldingDirtyPlate = false;

            dirtyPlate = null;
            characterHoldAbility = null;
            _nearbyCharacter = null; 
        }
    }

    private void GiveDirtyPlates() 
    {
        // 더러운 접시가 남아있고 근처 캐릭터가 있을 때
        if (DirtyPlateNum > 0 && _nearbyCharacter != null) 
        {
            UpdateDirtyPlateStatus();

            // 플레이어가 더러운 접시를 들고 있을 때
            if (isPlayerHoldingDirtyPlate) 
            {

                // 플레이어가 들고 있는 접시 개수에 더러운 접시 개수 추가
                dirtyPlate.DirtyPlateNum += DirtyPlateNum; 
            }
            else 
            {
                _nearbyCharacter.PhotonView.RPC("RequestSpawnDirtyPlateOnHand", RpcTarget.MasterClient); // 마스터 클라이언트에게 RPC 요청
                dirtyPlate = characterHoldAbility.gameObject.GetComponentInChildren<DirtyPlate>(); // 새로 생성된 더러운 접시 객체 가져오기

                if (dirtyPlate != null) 
                {
                    dirtyPlate.DirtyPlateNum = DirtyPlateNum;
                }
            }

            _pv.RPC(nameof(UpdatePlateNum), RpcTarget.AllBuffered, 0); // 모든 클라이언트에게 접시 개수 업데이트 RPC 호출
            UpdatePlates(); // 접시 상태 업데이트
        }
    }

    [PunRPC] // Photon RPC 메서드
    private void UpdatePlateNum(int newPlateNum) // 접시 개수를 업데이트하는 메서드
    {
        DirtyPlateNum = newPlateNum; // 새로운 접시 개수 설정
        UpdatePlates(); // 접시 상태 업데이트
        Debug.Log($"접시 개수 {DirtyPlateNum}개");
    }

    private void UpdateDirtyPlateStatus() // 더러운 접시 상태를 업데이트하는 메서드
    {
        if (characterHoldAbility != null) // 캐릭터 홀드 능력이 있을 때
        {
            dirtyPlate = characterHoldAbility.gameObject.GetComponentInChildren<DirtyPlate>(); // 더러운 접시 객체 가져오기
            isPlayerHoldingDirtyPlate = (dirtyPlate != null); // 플레이어가 더러운 접시를 들고 있는지 여부 설정
        }
    }
}
