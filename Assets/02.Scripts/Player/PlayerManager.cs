using System.Collections;
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

    protected override void Awake()
    {
        _spawnPoints = new List<Vector3>();
        foreach (var spawnTransform in SpawnPoints.GetComponentsInChildren<Transform>().Where(t => t != SpawnPoints.transform))
        {
            _spawnPoints.Add(spawnTransform.position);
        }
    }

    // 플레이어 생성을 책임짐
    public void CreatePlayer()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Count);
        var player = PhotonNetwork.Instantiate($"{PlayerSetting.CharacterType}Player_Prefab", _spawnPoints[randomIndex], Quaternion.identity);

        player.GetComponent<Player>().OnPlayerDeath += OnPlayerDeath;
        player.GetComponent<PlayerPresenter>().Init(UI_PlayerHUD);
        
        MinimapCamera.Target = player.transform;
    }

    private void RespawnPlayer(Player player)
    {
        if (!player.PlayerState.Is(EPlayerState.Dead) ||
            !player.PhotonView.IsMine)
        {
            return;
        }

        StartCoroutine(Respawn_Coroutine(player));
    }

    private IEnumerator Respawn_Coroutine(Player player)
    {
        yield return new WaitForSeconds(10f);
        player.PhotonView.RPC(nameof(Player.Respawn), RpcTarget.All, _spawnPoints[Random.Range(0, _spawnPoints.Count)]);
    }

    private void OnPlayerDeath(Player player)
    {
        // 사망시 발생하는 외부 이벤트
        // 아이템 생성
        // int randomCount = Random.Range(1, 4);
        // for (int i = 0; i < randomCount; i++)
        // {
        //     ItemObjectFactory.Instance.RequestCreate(EItemType.Score, player.transform.position + Vector3.up * 2f);
        // }
        int percent = Random.Range(0, 100);
        if (percent < 100)
        {
            ItemObjectFactory.Instance.RequestCreate(EItemType.Health, player.transform.position + new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f)));
        }
        percent = Random.Range(0, 100);
        if (percent < 100)
        {
            ItemObjectFactory.Instance.RequestCreate(EItemType.Stamina, player.transform.position + new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f)));
        }

        // 점수의 절반은 아이템으로
        int halfScore = ScoreManager.Instance.Score / 2;

        ItemObjectFactory.Instance.RequestCreateScoreItemsByScore(halfScore,
            player.transform.position + Vector3.up * 2f);
        
        ScoreManager.Instance.OnDeath();
        
        RespawnPlayer(player);
    }
}