using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Stat Stat;
    public PlayerState State;

    private void Awake()
    {
        State = new PlayerState();
    }
}