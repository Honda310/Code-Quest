/// <summary>
/// インベントリ内で「どのアイテム」を「何個」持っているかを管理するクラス。
/// </summary>
[System.Serializable]
public class CarryItem
{
    public SupportItem item;
    public int quantity;

    public CarryItem(SupportItem item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}