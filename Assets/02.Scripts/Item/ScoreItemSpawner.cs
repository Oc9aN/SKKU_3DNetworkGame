using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScoreItemSpawner : MonoBehaviour
{
    [SerializeField]
    private float _interval;

    [SerializeField]
    private float _range;
    
    private float _timer;

    private void Start()
    {
        _interval = Random.Range(_interval / 2f, _interval);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        _timer += Time.deltaTime;

        if (_timer >= _interval)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * _range;
            randomPosition.y = 3f;
            
            ItemObjectFactory.Instance.RequestCreate(EItemType.Score, randomPosition);

            _timer = 0f;
        }
    }
}