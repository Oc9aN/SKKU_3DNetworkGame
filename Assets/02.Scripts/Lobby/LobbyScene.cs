using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    public TMP_InputField NicknameInput;
    public TMP_InputField RoomNameInput;
    public Button MakeRoomButton;
    
    public GameObject MaleCharacter;
    public GameObject FemaleCharacter;
    public Button MaleButton;
    public Button FemaleButton;

    private void Awake()
    {
        MakeRoomButton.onClick.AddListener(OnClickMakeRoom);
        MaleButton.onClick.AddListener(OnClickMaleCharacter);
        FemaleButton.onClick.AddListener(OnClickFemaleCharacter);

        OnClickMaleCharacter();
    }

    // 방 만들기 함수를 호출
    private void OnClickMakeRoom()
    {
        MakeRoom();
    }

    private void MakeRoom()
    {
        string nickname = NicknameInput.text;
        string roomName = RoomNameInput.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName))
        {
            return;
        }
        
        PhotonNetwork.NickName = nickname;
        
        // Room 속성을 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true;    // 룸 입장 가능 여부
        roomOptions.IsVisible = true; // 로비(채널)에서 룸 목록에 노출 여부
        // Room 생성
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    
    private void OnClickMaleCharacter() => OnClickCharacterType(ECharacterType.Male);
    private void OnClickFemaleCharacter() => OnClickCharacterType(ECharacterType.Female);
    private void OnClickCharacterType(ECharacterType characterType)
    {
        PlayerSetting.CharacterType = characterType;
        MaleCharacter.SetActive(characterType == ECharacterType.Male);
        FemaleCharacter.SetActive(characterType == ECharacterType.Female);
    }
}
