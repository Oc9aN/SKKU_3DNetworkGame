using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ItemObjectFactory : MonoSingleton<ItemObjectFactory>
{
    // 포톤 네트워크 객체의 생명 주기
    // Player : 플레이어가 생성하고, 플레이어가 나가면 자동 삭제 (PhotonNetwork.Instantiate/Destroy)
    // Room : 방장이(서버 권한이 있는 자) 생성하고, 룸이 없어지면 삭제 (PhotonNetwork.InstantiateRoomObject/Destroy)
    
    private PhotonView _photonView;

    protected override void Awake()
    {
        base.Awake();
        _photonView = GetComponent<PhotonView>();
    }

    public void RequestCreate(EItemType itemType, Vector3 position)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CreateItem(itemType, position);
            return;
        }
        
        _photonView.RPC(nameof(CreateItem), RpcTarget.MasterClient, itemType, position);
    }
    
    [PunRPC]
    private void CreateItem(EItemType itemType, Vector3 position)
    {
        PhotonNetwork.InstantiateRoomObject($"{itemType}Item_Prefab", position, Quaternion.identity);
    }

    public void RequestDelete(int viewId)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            DeleteItem(viewId);
            return;
        }
        _photonView.RPC(nameof(DeleteItem), RpcTarget.MasterClient, viewId);
    }

    [PunRPC]
    private void DeleteItem(int viewId)
    {
        GameObject deleteItem = PhotonView.Find(viewId)?.gameObject;
        if (deleteItem == null) return;
        
        PhotonNetwork.Destroy(deleteItem);
    }
}