using System;
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
[RequireComponent(typeof(Collider))]
public class ItemObject : MonoBehaviourPun
{
    [SerializeField]
    private EItemType _itemType;
    public EItemType ItemType => _itemType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.AddScore(10);

            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}