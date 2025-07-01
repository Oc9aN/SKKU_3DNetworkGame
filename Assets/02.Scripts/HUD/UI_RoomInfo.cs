using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomInfo : MonoBehaviour
{
    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI PlayerCountText;
    public Button ExitButton;

    private void Start()
    {
        ExitButton.onClick.AddListener(OnClickExitButton);
        
        RoomManager.Instance.OnRoomDataChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        Room room = RoomManager.Instance.Room;
        if (room == null)
        {
            return;
        }
        
        RoomNameText.text = room.Name;
        PlayerCountText.text = $"{room.PlayerCount}/{room.MaxPlayers}";
    }

    public void OnClickExitButton()
    {
        Exit();
    }

    private void Exit()
    {
        
    }
}