using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour, IPunObservable
{
    [SerializeField]
    private PlayerStat _playerStat;
    public PlayerStat PlayerStat => _playerStat;
    public PlayerState PlayerState { get; private set; }

    private int _score;
    
    private Dictionary<Type, PlayerAbility> _abilityCache;
    
    private Animator _animator;
    private PhotonView _photonView;
    public PhotonView PhotonView => _photonView;
    private CharacterController _characterController;
    
    public event Action<Player> OnPlayerDeath;
    
    private void Awake()
    {
        PlayerState = new PlayerState();
        
        _abilityCache = new Dictionary<Type, PlayerAbility>();

        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (_playerStat.Health <= 0)
        {
            _photonView.RPC(nameof(TriggerAnimation), RpcTarget.All, "Dead");
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

    public void OnDead(int actorNumber)
    {
        OnPlayerDeath?.Invoke(this);
        
        _playerStat.SetHealth(0f);
        
        _characterController.enabled = false;
        PlayerState.ChangeState(EPlayerState.Dead);

        RoomManager.Instance.OnPlayerDeath(_photonView.Owner.ActorNumber, actorNumber);
        
        if (_photonView.IsMine)
        {
            _photonView.RPC(nameof(TriggerAnimation), RpcTarget.All, "Dead");
        }
        
        foreach (var ability in GetComponents<PlayerAbility>())
        {
            if (ability is IDisableOnDeath)
                ability.enabled = false;
        }
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_playerStat.Health);
        }
        else if (stream.IsReading)
        {
            float health = (float)stream.ReceiveNext();
            _playerStat.SetHealth(health);
        }
    }

    public void AddScore(int amount)
    {
        _score += amount;
    }
}