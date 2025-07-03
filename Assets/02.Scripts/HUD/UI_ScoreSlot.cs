using TMPro;
using UnityEngine;

public class UI_ScoreSlot : MonoBehaviour
{
    public TextMeshProUGUI RankText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI NickNameText;

    public void Set(int rank, string nickName, int score)
    {
        RankText.text = rank.ToString();
        NickNameText.text = nickName;
        scoreText.text = score.ToString("N0");
    }
}