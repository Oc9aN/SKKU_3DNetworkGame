using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public enum EItemType
{
    Score,
    Health,
    Stamina,
}

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(Rigidbody))]
public class ItemObject : MonoBehaviourPun
{
    [SerializeField]
    private EItemType _itemType;
    public EItemType ItemType => _itemType;
    
    [SerializeField]
    private ItemEffectBase _effect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();

            if (player.PlayerState.Is(EPlayerState.Dead))
            {
                return;
            }
            
            _effect.ApplyEffect(player);

            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}