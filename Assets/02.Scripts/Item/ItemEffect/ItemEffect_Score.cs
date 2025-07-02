using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/ScoreEffect")]
public class ItemEffect_Score : ItemEffectBase
{
    [SerializeField]
    private int amount;

    public override void ApplyEffect(Player player)
    {
        player.AddScore(amount);
    }
}