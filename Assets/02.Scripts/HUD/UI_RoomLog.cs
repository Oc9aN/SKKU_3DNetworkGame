using System;
using TMPro;
using UnityEngine;

public class UI_RoomLog : MonoBehaviour
{
    public TextMeshProUGUI LogText;

    private string _logMessage = "방에 입장했습니다.";

    private void Start()
    {
        RoomManager.Instance.OnPlayerEntered += PlayerEnterLog;
        RoomManager.Instance.OnPlayerExit += PlayerExitLog;
        RoomManager.Instance.OnPlayerKilled += PlayerDeathLog;
        Refresh();
    }

    private void Refresh()
    {
        LogText.text = _logMessage;
    }

    private void PlayerEnterLog(string playerName)
    {
        _logMessage += $"\n<color=#00ff00ff>{playerName}</color>님이 <color=#00ffffff>입장</color>하였습니다.";
        Refresh();
    }

    private void PlayerExitLog(string playerName)
    {
        _logMessage += $"\n<color=#00ff00ff>{playerName}</color>님이 <color=#ff0000ff>퇴장</color>하였습니다.";
        Refresh();
    }

    private void PlayerDeathLog(string playerName, string attackerName)
    {
        _logMessage += $"\n<color=#00ff00ff>{attackerName}</color>님이 <color=#ff00ffff>{playerName}</color>님을 <color=#ff0000ff>처치</color>하였습니다.";
        Refresh();
    }
}
