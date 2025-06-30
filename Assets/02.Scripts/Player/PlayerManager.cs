using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public UI_PlayerHUD  UI_PlayerHUD;
    public MinimapCamera MinimapCamera;
    // 플레이어 생성을 책임짐
    public void CreatePlayer()
    {
        var player = PhotonNetwork.Instantiate("Player_Prefab", Vector3.zero, Quaternion.identity);
        
        player.GetComponent<PlayerPresenter>().Init(UI_PlayerHUD);
        
        MinimapCamera.Target = player.transform;
    }
}