/// <summary>
/// インベントリ内で「どのアイテム」を「何個」持っているかを管理するクラス。
/// </summary>
[System.Serializable]
public class CarryItem
{
    public Item item;
    public int quantity;

    public CarryItem(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}