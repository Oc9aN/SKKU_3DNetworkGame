using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHead : MonoBehaviour
{
    public Slider HealthSlider;
    
    public void Refresh(PlayerStat playerStat)
    {
        HealthSlider.maxValue = playerStat.MaxHealth;
        HealthSlider.value = playerStat.Health;
    }
}