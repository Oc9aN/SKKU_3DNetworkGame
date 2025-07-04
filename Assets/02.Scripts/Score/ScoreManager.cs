using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreManager : MonoPunCallbacksSingleton<ScoreManager>
{
    [SerializeField]
    private ItemEffect_Score _scoreItemSO;
    
    [SerializeField]
    private int _killScore;
    public event Action OnScoreAdded;
    public event Action<List<KeyValuePair<string,int>>> OnDataChanged;
    
    private Dictionary<string, int> _scores = new Dictionary<string, int>();
    
    private int _score;
    public int Score => _score;

    private int _killCount;
    
    private int _totalScore;
    public int TotalScore => _totalScore;

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
        
        var sortedScore = _scores.OrderByDescending(x => x.Value).ToList();
        OnDataChanged?.Invoke(sortedScore);
    }

    private void Refresh()
    {
        Hashtable hashTable = new Hashtable();
        _totalScore = _killCount * _killScore + _score;
        hashTable.Add("Score", _totalScore);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);
        OnScoreAdded?.Invoke();
    }

    public void AddScore(int score)
    {
        _score += score;
        
        // 커스텀 프로퍼티 수정
        Refresh();
    }
    
    public void OnKill(int victimNumber)
    {
        _killCount++;
        
        // 피해자의 점수 절반 가져오기
        int halfScore = _scores[$"{PhotonNetwork.CurrentRoom.GetPlayer(victimNumber).NickName}_{victimNumber}"] / 2;
        Debug.Log(halfScore);
        _score += halfScore;
        
        // 커스텀 프로퍼티 수정
        Refresh();
    }

    public void OnDeath()
    {
        _score = 0;
        
        Refresh();
    }
}