/// <summary>
/// 消費アイテムクラス。使用時の効果IDを持ちます。
/// </summary>
[System.Serializable]
public class SupportItem : Item
{
    public int EffectID;   // 効果の種類
    public int EffectSize; // 効果の大きさ

    public SupportItem(int id, string name, int rarity, int effectId, int effectSize, ItemType type, string flavor)
        : base(id, name, rarity,type, flavor)
    {
        EffectID = effectId;
        EffectSize = effectSize;
    }
}