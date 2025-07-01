using System;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoPunCallbacksSingleton<RoomManager>
{
    private Room _room;
    public Room Room => _room;

    public event Action OnRoomDataChanged;
    
    public override void OnJoinedRoom()
    {
        // 플레이어 생성
        PlayerManager.Instance.CreatePlayer();

        // 룸 설정
        SetRoom();
        
        OnRoomDataChanged?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        OnRoomDataChanged?.Invoke();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        OnRoomDataChanged?.Invoke();
    }

    private void SetRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
    }
}