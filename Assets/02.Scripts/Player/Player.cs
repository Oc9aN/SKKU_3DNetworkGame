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
    
    private PhotonView _photonView;
    
    private void Awake()
    {
        PlayerState = new PlayerState();
        
        _abilityCache = new Dictionary<Type, PlayerAbility>();

        _photonView = GetComponent<PhotonView>();
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
        _playerStat.SetHealth(Mathf.Max(_playerStat.Health - damage, 0f));
        Debug.Log($"남은 체력{_playerStat.Health}");
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
}