/// <summary>
/// 防具（アクセサリ）クラス。防御力を持ちます。
/// </summary>
[System.Serializable]
public class Accessory : Item
{
    public int Def; // 防御力

    public Accessory(int id, string name, int rarity, int def, ItemType type, string flavor)
        : base(id, name, rarity,type, flavor)
    {
        Def = def;
    }
}