using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStat PlayerStat;
    public PlayerState PlayerState { get; private set; }
    
    private Dictionary<Type, PlayerAbility> _abilityCache;
    
    private void Awake()
    {
        PlayerState = new PlayerState();
        
        _abilityCache = new Dictionary<Type, PlayerAbility>();
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
}