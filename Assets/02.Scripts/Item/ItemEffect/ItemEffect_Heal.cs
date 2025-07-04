using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/HealEffect")]
public class ItemEffect_Heal : ItemEffectBase
{
    [SerializeField]
    private float _amount;

    public override void ApplyEffect(Player player)
    {
        player.AddHealth(_amount);
    }
}