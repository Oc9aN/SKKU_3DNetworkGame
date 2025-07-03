using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreManager : MonoPunCallbacksSingleton<ScoreManager>
{
    public event Action<Dictionary<string, int>> OnDataChanged;
    
    private Dictionary<string, int> _scores = new Dictionary<string, int>();
    
    private int _score;
    
    public int Score => _score;

    public override void OnJoinedRoom()
    {
        // 커스텀 프로퍼티 초기화
        Refresh();
    }

    // 플레이어의 커스텀 프로퍼티가 수정되면 호출되는 콜백함수
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        var roomPlayers = PhotonNetwork.PlayerList;
        foreach (var player in roomPlayers)
        {
            if (player.CustomProperties.ContainsKey("Score"))
            {
                _scores[$"{player.NickName}_{player.ActorNumber}"] = (int)player.CustomProperties["Score"];
            }
        }
        
        OnDataChanged?.Invoke(_scores);
    }

    private void Refresh()
    {
        Hashtable hashTable = new Hashtable();
        hashTable.Add("Score", _score);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);
    }

    public void AddScore(int score)
    {
        _score += score;
        
        // 커스텀 프로퍼티 수정
        Refresh();
    }
}