using System;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    private Player _player;
    private PlayerView _playerView;

    private void Start()
    {
        _player = GetComponent<Player>();
        _playerView = GetComponent<PlayerView>();
        
        _player.PlayerStat.OnDataChanged += _playerView.Refresh;
    }
}