using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public UI_PlayerHUD  UI_PlayerHUD;
    public MinimapCamera MinimapCamera;
    public Transform SpawnPoints;

    private List<Vector3> _spawnPoints;

    private void Start()
    {
        _spawnPoints = new List<Vector3>();
        foreach (var spawnTransform in SpawnPoints.GetComponentsInChildren<Transform>().Where(t => t != this.transform))
        {
            _spawnPoints.Add(spawnTransform.position);
        }
    }

    // 플레이어 생성을 책임짐
    public void CreatePlayer()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Count);
        var player = PhotonNetwork.Instantiate("Player_Prefab", _spawnPoints[randomIndex], Quaternion.identity);
        
        player.GetComponent<PlayerPresenter>().Init(UI_PlayerHUD);
        
        MinimapCamera.Target = player.transform;
    }
}