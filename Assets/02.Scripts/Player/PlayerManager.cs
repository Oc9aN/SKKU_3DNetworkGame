using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 플레이어 생성을 책임짐
    public static void CreatePlayer()
    {
        var player = PhotonNetwork.Instantiate("Player_Prefab", Vector3.zero, Quaternion.identity); 
        Player playerComponent = player.GetComponent<Player>();
        PlayerView playerViewComponent = player.GetComponent<PlayerView>();
        new PlayerPresenter(playerComponent, playerViewComponent);
    }
}