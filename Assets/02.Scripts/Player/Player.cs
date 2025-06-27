using System;
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
    
    private PhotonView _photonView;
    
    private void Awake()
    {
        PlayerState = new PlayerState();
        
        _photonView = GetComponent<PhotonView>();
        _abilityCache = new Dictionary<Type, PlayerAbility>();
    }

    private void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        
        if (PlayerState.Is(EPlayerState.Idle))
        {
            float staminaRecoverAmount = _playerStat.StaminaRecovery * Time.deltaTime;
            _playerStat.StaminaRecover(staminaRecoverAmount);
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
}