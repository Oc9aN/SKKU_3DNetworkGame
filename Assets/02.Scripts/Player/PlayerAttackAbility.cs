using System;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    private float _attackCoolTimer;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _attackCoolTimer -= Time.deltaTime;
        if (_attackCoolTimer <= 0f && Input.GetMouseButtonDown(0))
        {
            // 공격 1~3 랜덤 공격
            int random = UnityEngine.Random.Range(1, 4);
            _animator.SetTrigger($"Attack{random}");
            _attackCoolTimer = _player.Stat.AttackCoolTime;
            _player.State.ChangeState(EPlayerState.Attack);
        }
    }

    public void OnAttackEnd()
    {
        _player.State.ChangeState(EPlayerState.Idle);
    }
}
