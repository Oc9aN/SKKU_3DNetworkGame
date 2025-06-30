using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHUD : MonoBehaviour
{
    public Slider HealthSlider;
    public Slider StaminaSlider;
    
    public void Refresh(PlayerStat playerStat)
    {
        StaminaSlider.maxValue = playerStat.MaxStamina;
        StaminaSlider.value = playerStat.Stamina;
        
        HealthSlider.maxValue = playerStat.MaxHealth;
        HealthSlider.value = playerStat.Health;
    }
}
