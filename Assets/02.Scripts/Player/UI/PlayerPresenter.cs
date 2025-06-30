using System;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    private Player _player;
    private UI_PlayerHead _uiPlayerHead;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _uiPlayerHead = GetComponent<UI_PlayerHead>();
    }

    // 각 플레이어 객체가 머리 위 UI 연결
    private void Start()
    {
        _player.PlayerStat.OnDataChanged += _uiPlayerHead.Refresh;
    }

    // 플레이어 생성 후 HUD 연결
    public void Init(UI_PlayerHUD uiPlayerHUD)
    {
        _player.PlayerStat.OnDataChanged += uiPlayerHUD.Refresh;
    }
}