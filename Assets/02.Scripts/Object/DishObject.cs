using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DishState
{
    Dirty,
    Clean
}

public class DishObject : IHoldable
{
    public override Vector3 DropOffset => new Vector3(0.3f, 0.1f, 0f);

    public DishState State;
    public Transform StartPosition;
    private PhotonView _pv;
    private bool _isInitializing = false;
    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (StartPosition != null)
        {
            _isInitializing = true; 
            Place(StartPosition);
            _isInitializing = false; 
        }

    }
    private void Update()
    {
        if (_holdCharacter != null)
        {
            transform.localPosition = Vector3.zero;
        }
    }
    public override void Hold(Character character, Transform handTransform)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_pv.OwnerActorNr != character.PhotonView.OwnerActorNr)
            {
                _pv.TransferOwnership(character.PhotonView.OwnerActorNr);
            }
        }
       // Debug.Log("플레이어가 접시를 들고있다.");

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = false;
        transform.parent = handTransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _holdCharacter = character;
        SoundManager.Instance.PlayAudio("Dish", false, false);
    }

    public override void Place(Transform place)
    {
        if (place == null)
        {
            return;
        }

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = true;
        transform.rotation = place.rotation;
        transform.position = place.position;
        //placeRotation = Quaternion.Euler(0, 0, 0);
        transform.SetParent(place);
        _holdCharacter = null;

        if (!_isInitializing)
        {
            SoundManager.Instance.PlayAudio("DishPlace", false, false);
        }
    }

    public override void UnHold(Vector3 dropPosition, Quaternion dropRotation)
    {

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<BoxCollider>().enabled = false;
        // 저장한 위치와 회전으로 음식 배치
        transform.position = dropPosition;
        transform.rotation = dropRotation;

        transform.parent = null;
        //각 아이템이 떼어질 때 해줄 초기화 로직
        _holdCharacter = null;

        SoundManager.Instance.PlayAudio("DishPlace", false, false);
    }
}
