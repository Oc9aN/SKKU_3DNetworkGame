using UnityEngine;

public class PlayerStaminaAbility : PlayerAbility
{
    private void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        
        if (!_player.PlayerState.Is(EPlayerState.Idle))
        {
            return;
        }
        
        float staminaRecoverAmount = _player.PlayerStat.StaminaRecovery * Time.deltaTime;
        _player.PlayerStat.StaminaRecover(staminaRecoverAmount);
    }
}