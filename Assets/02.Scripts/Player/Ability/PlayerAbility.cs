using System;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    protected Player _player { get; private set; }

    protected virtual void Awake()
    {
        _player = GetComponent<Player>();
    }
}
