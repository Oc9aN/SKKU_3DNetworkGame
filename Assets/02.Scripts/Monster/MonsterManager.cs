using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MonsterManager : MonoPunCallbacksSingleton<MonsterManager>
{
    [SerializeField]
    private Transform _spawnPoint;
    
    [SerializeField]
    private List<Transform> _patrolPoints;
    
    private PhotonView _photonView;
    
    public override void OnCreatedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 곰 생성
            var monster = PhotonNetwork.InstantiateRoomObject("Monster_Prefab", _spawnPoint.position, Quaternion.identity);

            monster.GetComponent<Monster>().Init(_patrolPoints);
        }
    }
}
