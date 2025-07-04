using System;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoPunCallbacksSingleton<RoomManager>
{
    private Room _room;
    public Room Room => _room;

    public event Action OnRoomDataChanged;
    public event Action<string> OnPlayerEntered;
    public event Action<string> OnPlayerExit;
    public event Action<string, string> OnPlayerKilled;

    bool _isInitialized = false;

    public override void OnJoinedRoom()
    {
        Init();
    }

    private void Start()
    {
        Init();
    }
    
    private void Init()
    {
        if (_isInitialized)
        {
            return;
        }

        if (!PhotonNetwork.InRoom)
        {
            return;
        }

        _isInitialized = true;
        // 플레이어 생성
        PlayerManager.Instance.CreatePlayer();

        // 룸 설정
        SetRoom();
        
        OnRoomDataChanged?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        OnRoomDataChanged?.Invoke();
        OnPlayerEntered?.Invoke(newPlayer.NickName + "_" + newPlayer.ActorNumber);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        OnRoomDataChanged?.Invoke();
        OnPlayerExit?.Invoke(otherPlayer.NickName + "_" + otherPlayer.ActorNumber);
    }

    public void OnPlayerDeath(int playerNumber, int attackerNumber)
    {
        string playerName = _room.Players[playerNumber].NickName + playerNumber;
        string attackerName = _room.Players[attackerNumber].NickName + attackerNumber;
        OnPlayerKilled?.Invoke(playerName, attackerName);
    }

    private void SetRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
    }
}