using System;
using Photon.Pun;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.AddScore(10);
            
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
