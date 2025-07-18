using System;
using UnityEngine;

// Photon API 네임스페이스
using Photon.Pun;
using Photon.Realtime;

// 역할: 포톤 서버 관리자(서버 연결, 로비 입장, 방 입장, 게임 입장)
public class PhotonServerManager : MonoPunCallbacksSingleton<PhotonServerManager>
{
    // MonoBehaviourPunCallbacks : 유니티 이벤트 말고도 PUN 서버 이벤트를 받을 수 있다.
    private readonly string _gameVersion = "1.0.0";
    private string _nickname = "Ocean";

    private void Start()
    {
        // 설정
        // 0. 데이터 송수신 빈도를 매 초당 30회로 설정한다. (기본은 10)
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        // 1. 버전 : 버전이 다르면 다른 서버로 접속이 된다.
        PhotonNetwork.GameVersion = _gameVersion;
        // 2. 닉네임 : 게임에서 사용할 사용자의 별명 (중복 가능 -> 판별을 위해서는 UserId)
        PhotonNetwork.NickName = _nickname;

        // 방장이 로드한 씬으로 다른 참여자가 똑같이 이동하게끔 동기화 해주는 옵션
        // 방장: 방을 만든 소유자이자 "마스터 클라이언트" (방마다 한병의 마스터 클라이언트가 존재)
        PhotonNetwork.AutomaticallySyncScene = true;

        // 설정 값으로 서버 접속 시도
        // 네임 서버 접속 -> 방 목록이 있는 마스터 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnected()
    {
        Debug.Log("네임 서버 접속 완료");
        Debug.Log(PhotonNetwork.CloudRegion);
    }

    // 포톤 마스터 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 접속 완료");
        Debug.Log($"InLobby: {PhotonNetwork.InLobby}");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비(채널) 입장 완료!");
        Debug.Log($"InLobby: {PhotonNetwork.InLobby}");

        // 랜덤 방에 들어간다.
        // PhotonNetwork.JoinRandomRoom();
    }

    // 방 입장 후 호출되는 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"방 입장 {PhotonNetwork.InRoom} : {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"플레이어 수: {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleScene");
        }
        
        // var roomPlayers = PhotonNetwork.CurrentRoom.Players;
        // foreach (var pair in roomPlayers)
        // {
        //     Debug.Log($"{pair.Value.ActorNumber}: {pair.Value.NickName}");
        //     // ActorNumber는 방에서 플레이어의 구별 ID
        //     
        //     // 진짜 고유 아이디
        //     Debug.Log($"{pair.Value.UserId}"); // 유저의 고유 아이디
        // }
    }

    // 방 입장에 실패하면 호출되는 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤 방 입장에 실패했습니다: {returnCode} : {message}");

        // // Room 속성을 정의
        // RoomOptions roomOptions = new RoomOptions();
        // roomOptions.MaxPlayers = 20;
        // roomOptions.IsOpen = true;    // 룸 입장 가능 여부
        // roomOptions.IsVisible = true; // 로비(채널)에서 룸 목록에 노출 여부
        //
        // // Room 생성
        // PhotonNetwork.CreateRoom("test", roomOptions);
    }

    // 방 생성 실패에 호출되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"방 생성에 실패했습니다: {returnCode} : {message}");
    }
    
    // 룸 생성에 성공했을때 호출되는 함수
    public override void OnCreatedRoom()
    {
        Debug.Log($"방 생성 성공했습니다: {PhotonNetwork.CurrentRoom.Name}");
    }
}