using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    public Slider StaminaSlider;
    
    public void Refresh(PlayerStat playerStat)
    {
        StaminaSlider.maxValue = playerStat.MaxStamina;
        StaminaSlider.value = playerStat.Stamina;
    }
}