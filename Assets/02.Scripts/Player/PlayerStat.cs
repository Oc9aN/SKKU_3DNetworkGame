using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    // ──────────────── 체력 관련 ────────────────
    [Header("체력")]
    [SerializeField]
    private float _maxHealth;

    public float MaxHealth => _maxHealth;

    [SerializeField]
    private float _health;

    public float Health => _health;

    // ──────────────── 스태미나 관련 ────────────────
    [Header("스테미나")]
    [SerializeField]
    private float _maxStamina;

    public float MaxStamina => _maxStamina;

    [SerializeField]
    private float _stamina;

    public float Stamina => _stamina;

    [SerializeField]
    private float _staminaRecovery;

    public float StaminaRecovery => _staminaRecovery;

    [SerializeField]
    private float _runStaminaRate;

    public float RunStaminaRate => _runStaminaRate;

    // ──────────────── 이동 관련 ────────────────
    [Header("이동")]
    [SerializeField]
    private float _moveSpeed;

    public float MoveSpeed => _moveSpeed;

    [SerializeField]
    private float _runSpeed;

    public float RunSpeed => _runSpeed;

    [SerializeField]
    private float _rotationSpeed;

    public float RotationSpeed => _rotationSpeed;

    [SerializeField]
    private float _jumpPower;

    public float JumpPower => _jumpPower;
    
    [SerializeField]
    private float _jumpStamina;

    public float JumpStamina => _jumpStamina;

    // ──────────────── 전투 관련 ────────────────
    [Header("전투")]
    [SerializeField]
    private float _attackSpeed;

    public float AttackSpeed => _attackSpeed;

    [SerializeField]
    private float _attackStamina;

    public float AttackStamina => _attackStamina;
    
    [SerializeField]
    private float _attackDamage;

    public float AttackDamage => _attackDamage;

    // ──────────────── 이벤트 ────────────────
    public event Action<PlayerStat> OnDataChanged;
    public event Action OnStaminaEmpty;

    // ──────────────── 메서드 ────────────────
    public bool TryUseStamina(float amount)
    {
        if (_stamina - amount < 0)
        {
            OnStaminaEmpty?.Invoke();
            _stamina = 0f;
            return true;
        }

        _stamina -= amount;
        OnDataChanged?.Invoke(this);
        return true;
    }

    public void StaminaRecover(float amount)
    {
        _stamina += amount;
        if (_stamina > _maxStamina)
            _stamina = _maxStamina;

        OnDataChanged?.Invoke(this);
    }
    
    public void SetStamina(float stamina)
    {
        _stamina = stamina;

        OnDataChanged?.Invoke(this);
    }

    public void SetHealth(float health)
    {
        _health = health;
        OnDataChanged?.Invoke(this);
    }
}