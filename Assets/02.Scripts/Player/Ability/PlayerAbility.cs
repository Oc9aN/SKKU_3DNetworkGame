using System;
using Photon.Pun;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    protected Player _player { get; private set; }
    protected PhotonView _photonView { get; private set; }

    protected virtual void Awake()
    {
        _player = GetComponent<Player>();
        _photonView = GetComponent<PhotonView>();
    }
}
