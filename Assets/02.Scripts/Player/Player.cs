using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviour, IPunObservable, IDamaged
{
    [SerializeField]
    private PlayerStat _playerStat;
    public PlayerStat PlayerStat => _playerStat;
    public PlayerState PlayerState { get; private set; }
    
    private Dictionary<Type, PlayerAbility> _abilityCache;
    
    private PhotonView _photonView;
    
    private void Awake()
    {
        PlayerState = new PlayerState();
        
        _abilityCache = new Dictionary<Type, PlayerAbility>();

        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        
        // _playerStat.OnStaminaEmpty += OnStaminaEmpty;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_playerStat.Stamina);
        }
        else
        {
            float stamina = (float)stream.ReceiveNext();

            _playerStat.SetStamina(stamina);
        }
    }

    [PunRPC]
    public void Damaged(float damage)
    {
        _playerStat.SetHealth(Mathf.Max(_playerStat.Health - damage, 0f));
        Debug.Log($"남은 체력{_playerStat.Health}");
    }

    private void OnStaminaEmpty()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        if (PlayerState.Is(EPlayerState.Burnout))
        {
            return;
        }
        Debug.Log("탈진");
        StartCoroutine(Burnout_Coroutine());
    }

    private IEnumerator Burnout_Coroutine()
    {
        PlayerState.ChangeState(EPlayerState.Burnout);
        yield return new WaitForSeconds(3f);
        PlayerState.ChangeState(EPlayerState.Idle);
    }

    public bool TryUseStamina(float amount)
    {
        if (PlayerState.Is(EPlayerState.Burnout))
        {
            return false;
        }
        
        return _playerStat.TryUseStamina(amount);
    }
}