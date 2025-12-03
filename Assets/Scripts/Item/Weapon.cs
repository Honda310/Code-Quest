/// <summary>
/// 武器アイテムクラス。攻撃力を持ちます。
/// </summary>
[System.Serializable]
public class Weapon : Item
{
    public int Atk;       // 攻撃力
    public int TimeLimit; // クイズ回答のボーナス時間

    public Weapon(string id, string name, int rarity, int atk, int timeLimit, string flavor)
        : base(id, name, rarity, flavor)
    {
        Atk = atk;
        TimeLimit = timeLimit;
    }
}