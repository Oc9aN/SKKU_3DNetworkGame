using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviour, IDamaged
{
    [SerializeField]
    private PlayerStat _playerStat;
    public PlayerStat PlayerStat => _playerStat;
    public PlayerState PlayerState { get; private set; }
    
    private Dictionary<Type, PlayerAbility> _abilityCache;
    
    private Animator _animator;
    private PhotonView _photonView;
    public PhotonView PhotonView => _photonView;
    private CharacterController _characterController;
    
    private void Awake()
    {
        PlayerState = new PlayerState();
        
        _abilityCache = new Dictionary<Type, PlayerAbility>();

        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        if (transform.position.y <= -10)
        {
            _photonView.RPC(nameof(Damaged), RpcTarget.AllBuffered, float.MaxValue);
        }
    }

    public T GetAbility<T>() where T : PlayerAbility
    {
        if (_abilityCache.TryGetValue(typeof(T), out var ability))
        {
            return ability as T;
        }
        
        // 필요할 때 초기화
        ability = GetComponent<T>();

        if (ability != null)
        {
            _abilityCache[ability.GetType()] = ability;
            
            return ability as T;
        }
        
        throw new Exception($"PlayerAbility {typeof(T)}컴포넌트를 찾을 수 없습니다.");
        
        return null;
    }

    [PunRPC]
    public void Damaged(float damage)
    {
        if (PlayerState.Is(EPlayerState.Dead))
        {
            return;
        }
        
        if (_playerStat.Health - damage <= 0f)
        {
            // 사망
            _playerStat.SetHealth(0f);
            OnDead();
            Debug.Log("사망");
            return;
        }
        
        _playerStat.SetHealth(Mathf.Max(_playerStat.Health - damage, 0f));
        Debug.Log($"남은 체력{_playerStat.Health}");
    }
    
    [PunRPC]
    public void DamagedEvent(float damage)
    {
        if (_playerStat.Health - damage <= 0f)
        {
            // 사망
        }
    }

    public bool TryUseStamina(float amount)
    {
        if (PlayerState.Is(EPlayerState.Burnout))
        {
            return false;
        }

        if (PlayerStat.TryUseStamina(amount))
        {
            return true;
        }
        
        Debug.Log("탈진");
        PlayerState.ChangeState(EPlayerState.Burnout);
        PlayerStat.SetStamina(0f);
        StartCoroutine(Burnout_Coroutine());
        return true;
    }

    private IEnumerator Burnout_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        PlayerState.ChangeState(EPlayerState.Live);
    }

    private void OnDead()
    {
        _characterController.enabled = false;
        PlayerState.ChangeState(EPlayerState.Dead);
        if (_photonView.IsMine)
        {
            _photonView.RPC(nameof(TriggerAnimation), RpcTarget.All, "Dead");
        }
        
        foreach (var ability in GetComponents<PlayerAbility>())
        {
            if (ability is IDisableOnDeath)
                ability.enabled = false;
        }
        
        PlayerManager.Instance.RespawnPlayer(this);
    }

    [PunRPC]
    public void Respawn(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        _characterController.enabled = true;
        PlayerState.ChangeState(EPlayerState.Live);
        _animator.SetTrigger("Respawn");
        
        _playerStat.SetStamina(_playerStat.MaxStamina);
        _playerStat.SetHealth(_playerStat.MaxHealth);
        
        foreach (var ability in GetComponents<PlayerAbility>())
        {
            if (ability is IDisableOnDeath)
                ability.enabled = true;
        }
    }

    [PunRPC]
    private void TriggerAnimation(string trigger)
    {
        _animator.SetTrigger(trigger);
    }
}