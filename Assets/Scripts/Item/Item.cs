/// <summary>
/// すべてのアイテムの基本となる抽象クラスです。
/// IDは文字列型(String)で管理します。
/// </summary>
[System.Serializable]
public abstract class Item
{
    public int ItemID;   // 例: "Wb001"
    public string ItemName; // 名前
    public int Rarity;      // レア度
    public string Flavor;   // 説明文
    public ItemType Type;

    public Item(int id, string name, int rarity, ItemType type, string flavor)
    {
        ItemID = id;
        ItemName = name;
        Rarity = rarity;
        Type = type;
        Flavor = flavor;
    }
    public enum ItemType
    {
        Weapon,
        Accessory,
        Support
    }
}