using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/StaminaEffect")]
public class ItemEffect_Stamina : ItemEffectBase
{
    [SerializeField]
    private float amount;

    public override void ApplyEffect(Player player)
    {
        player.AddStamina(amount);
    }
}