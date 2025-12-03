/// <summary>
/// 消費アイテムクラス。使用時の効果IDを持ちます。
/// </summary>
[System.Serializable]
public class SupportItem : Item
{
    public int EffectID;   // 効果の種類
    public int EffectSize; // 効果の大きさ

    public SupportItem(string id, string name, int rarity, int effectId, int effectSize, string flavor)
        : base(id, name, rarity, flavor)
    {
        EffectID = effectId;
        EffectSize = effectSize;
    }

    // アイテム使用処理
    public void Use(Player player, EffectManager manager)
    {
        manager.ApplyEffect(EffectID, player, EffectSize);
    }
}