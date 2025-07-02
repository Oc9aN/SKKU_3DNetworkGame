using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.AddScore(10);
            
            Destroy(gameObject);
        }
    }
}
