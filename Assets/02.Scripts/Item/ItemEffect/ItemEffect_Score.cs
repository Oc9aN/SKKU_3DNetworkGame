using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/ScoreEffect")]
public class ItemEffect_Score : ItemEffectBase
{
    [SerializeField]
    private int _score;

    public int Score => _score;

    public override void ApplyEffect(Player player)
    {
        ScoreManager.Instance.AddScore(_score);
    }
}