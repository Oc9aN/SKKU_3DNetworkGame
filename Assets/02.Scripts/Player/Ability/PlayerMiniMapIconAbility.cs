using System;
using UnityEngine;

public class PlayerMiniMapIconAbility : PlayerAbility
{
    public GameObject PlayerMiniMapIcon;
    public GameObject EnemyMiniMapIcon;

    private void Start()
    {
        if (_photonView.IsMine)
        {
            PlayerMiniMapIcon.SetActive(true);
            EnemyMiniMapIcon.SetActive(false);
        }
        else
        {
            PlayerMiniMapIcon.SetActive(false);
            EnemyMiniMapIcon.SetActive(true);
        }
    }
}
