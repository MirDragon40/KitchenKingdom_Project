using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player = Photon.Realtime.Player;

public class GatherManager : MonoBehaviourPunCallbacks
{
    public static GatherManager Instance { get; private set; }
    [HideInInspector]
    public bool IsAllReady = false;

    private void Awake()
    {

        if (Instance == null)
        { 
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public struct PlayerSlot
    {
        public bool IsOccupied;
        public bool IsReady;
        public string PlayerId;
    }

    public PlayerSlot[] slots = new PlayerSlot[4];

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Player entered room: " + newPlayer.NickName);
        AssignSlot(newPlayer);
    }

    private void AssignSlot(Photon.Realtime.Player player)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsOccupied)
            {
                slots[i].IsOccupied = true;
                slots[i].PlayerId = player.UserId;
                photonView.RPC("UpdateSlot", RpcTarget.AllBuffered, i, true, player.UserId);
                break;
            }
        }
    }

    private bool AllPlayersReady()
    {
        foreach (PlayerSlot slot in slots)
        {
            if (!slot.IsOccupied || !slot.IsReady)
            {
                return false;
            }
        }
        return true;
    }

    [PunRPC]
    public void PlayerReady(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogError("Invalid slot index");
            return;
        }

        if (!slots[slotIndex].IsOccupied)
        {
            Debug.LogError("Slot is not occupied");
            return;
        }

        slots[slotIndex].IsReady = true;

        if (AllPlayersReady())
        {
            IsAllReady = true;
        }
        else
        {
            IsAllReady = false;
        }
    }

    [PunRPC]
    public void UpdateSlot(int slotIndex, bool isOccupied, string playerId)
    {
        slots[slotIndex].IsOccupied = isOccupied;
        slots[slotIndex].PlayerId = playerId;
    }

    private void ProceedToNextScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("NextSceneName"); // "NextSceneName"을 실제 씬 이름으로 변경
        }
    }

    public void OnReadyButtonClicked()
    {
        string userId = PhotonNetwork.LocalPlayer.UserId;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].PlayerId == userId)
            {
                photonView.RPC("PlayerReady", RpcTarget.AllBuffered, i);
                break;
            }
        }
    }
}
