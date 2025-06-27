using System;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    private float _attackTimer;
    private Animator _animator;

    private CharacterController _characterController;

    private void Start()
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        _attackTimer += Time.deltaTime;
        var attackStaminaCost = _player.PlayerStat.AttackStamina;
        if (_attackTimer >= (1f / _player.PlayerStat.AttackSpeed)
            && Input.GetMouseButtonDown(0)
            && _characterController.isGrounded
            && _player.PlayerStat.TryUseStamina(attackStaminaCost))
        {
            // 공격 1~3 랜덤 공격
            int random = UnityEngine.Random.Range(1, 4);
            _animator.SetTrigger($"Attack{random}");
            _attackTimer = 0f;
            _player.PlayerState.ChangeState(EPlayerState.Attack);
        }
    }

    public void OnAttackEnd()
    {
        _player.PlayerState.ChangeState(EPlayerState.Idle);
    }
}