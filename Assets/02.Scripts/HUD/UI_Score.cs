using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    public List<UI_ScoreSlot> Slots;
    public UI_ScoreSlot MySlot;

    private void Start()
    {
        ScoreManager.Instance.OnDataChanged += Refresh;
    }

    private void Refresh(Dictionary<string, int> dict)
    {
        // 1~4위 표시
        var sortedScore = dict.OrderByDescending(x => x.Value).ToList();
        for (int i = 0; i < Slots.Count; i++)
        {
            if (i < sortedScore.Count)
            {
                Slots[i].gameObject.SetActive(true);
                Slots[i].Set(i + 1, sortedScore[i].Key, sortedScore[i].Value);
            }
            else
            {
                Slots[i].gameObject.SetActive(false);
            }
        }

        string nickName = $"{PhotonNetwork.NickName}_{PhotonNetwork.LocalPlayer.ActorNumber}";
        int index = sortedScore.FindIndex(x => x.Key == nickName);
        if (index < 0)
            return;
        MySlot.Set(index + 1, sortedScore[index].Key, sortedScore[index].Value);
    }
}