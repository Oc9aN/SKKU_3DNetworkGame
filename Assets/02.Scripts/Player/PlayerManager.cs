using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public MinimapCamera MinimapCamera;
    // 플레이어 생성을 책임짐
    public void CreatePlayer()
    {
        var player = PhotonNetwork.Instantiate("Player_Prefab", Vector3.zero, Quaternion.identity); 
        
        MinimapCamera.Target = player.transform;
    }
}