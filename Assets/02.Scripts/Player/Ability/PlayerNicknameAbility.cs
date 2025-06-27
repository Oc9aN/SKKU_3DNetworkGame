using TMPro;
using UnityEngine;

public class PlayerNicknameAbility : PlayerAbility
{
    public TextMeshProUGUI NickNameTextUI;
    
    void Start()
    {
        NickNameTextUI.text = $"{_photonView.Owner.NickName}_{_photonView.Owner.ActorNumber}";

        if (_photonView.IsMine)
        {
            NickNameTextUI.color = Color.green;
        }
        else
        {
            NickNameTextUI.color = Color.red;
        }
    }
}
