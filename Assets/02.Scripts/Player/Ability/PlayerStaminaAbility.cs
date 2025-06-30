using UnityEngine;

public class PlayerStaminaAbility : PlayerAbility, IDisableOnDeath
{
    private void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        
        if (!CanRecover())
        {
            return;
        }
        
        float staminaRecoverAmount = _player.PlayerStat.StaminaRecovery * Time.deltaTime;
        _player.PlayerStat.StaminaRecover(staminaRecoverAmount);
    }

    private bool CanRecover()
    {
        return !_player.GetAbility<PlayerAttackAbility>().IsAttacking &&
               !_player.GetAbility<PlayerMoveAbility>().IsRunning &&
               !_player.PlayerState.Is(EPlayerState.Burnout);
    }
}