using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviour, IPunObservable
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

    public void OnDead(int killerActorNumber)
    {
        // 이미 죽은 상태면 무시
        if (PlayerState.Is(EPlayerState.Dead))
            return;

        // 체력 0, 상태 변경
        _playerStat.SetHealth(0f);
        PlayerState.ChangeState(EPlayerState.Dead);

        // 캐릭터 비활성화
        _characterController.enabled = false;

        // 사망 시 비활성화될 어빌리티 처리
        foreach (var ability in GetComponents<PlayerAbility>())
        {
            if (ability is IDisableOnDeath)
                ability.enabled = false;
        }

        // 내 캐릭터일 때만 애니메이션 트리거
        if (_photonView.IsMine)
        {
            _photonView.RPC(nameof(TriggerAnimation), RpcTarget.All, "Dead");
        }

        // 킬러가 있을 때만 점수/킬 처리
        if (killerActorNumber > 0)
        {
            // 방 전체에 사망 이벤트 통보
            RoomManager.Instance.OnPlayerDeath(_photonView.Owner.ActorNumber, killerActorNumber);

            // 내 캐릭터일 경우 킬러에게 킬 이벤트 전달
            if (_photonView.IsMine)
            {
                var killer = PhotonNetwork.CurrentRoom.GetPlayer(killerActorNumber);
                if (killer != null)
                {
                    _photonView.RPC(nameof(OnKill), killer, _photonView.Owner.ActorNumber);
                }
            }
        }

        // 공통 사망 이벤트
        OnPlayerDeath?.Invoke(this);
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

    public void AddStamina(float amount)
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        _playerStat.SetStamina(_playerStat.Stamina + amount);
    }

    public void AddHealth(float amount)
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        _playerStat.SetHealth(_playerStat.Health + amount);
    }

    [PunRPC]
    public void OnKill(int victimNumber)
    {
        ScoreManager.Instance.OnKill(victimNumber);
    }
}