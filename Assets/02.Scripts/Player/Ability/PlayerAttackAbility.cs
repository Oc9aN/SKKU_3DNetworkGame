using System;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    private float _attackTimer;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= (1f / _player.PlayerStat.AttackSpeed) && Input.GetMouseButtonDown(0))
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
