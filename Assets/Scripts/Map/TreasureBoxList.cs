using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
/// <summary>
/// 宝箱の開閉状態と中身を管理するクラスです。
/// </summary>
public class TreasureBoxData
{
    public int itemId;
    public bool accessAble;
}
[System.Serializable]
public class TreasureBoxList : MonoBehaviour
{
    public Dictionary<int, TreasureBoxData> TreasureBoxTable = new Dictionary<int, TreasureBoxData>();
    private DataManager dataManager;
    private Inventory inventory;
    void Start()
    {
        dataManager = GameManager.Instance.dataManager;
        inventory = GameManager.Instance.inventory;
    }
    public void TreasureBoxDataCreate()
    {
        
    }
    /// <summary>
    /// 可読性のためだけに存在するメソッド。
    /// 宝箱ID+宝箱の中身のアイテムIDだけでクラスを作れた方が可読性がいいだろ！
    /// 最初から中身が空の宝箱を便宜上宝箱にする意味もないしね。
    /// </summary>
    /// <param name="TreasureBoxId"></param>
    /// <param name="ItemId"></param>
    public void CreateTreasureBox(int TreasureBoxId,int ItemId)
    {
        TreasureBoxTable[TreasureBoxId] = new TreasureBoxData
        {
            itemId = ItemId,
            accessAble = true
        };
    }
    public string OpenTreasureBox(int TreasureBoxId,int ColorId)
    {
        int id = TreasureBoxTable[TreasureBoxId].itemId;
        if (id <= 20000 && id > 10000)
        {
            Item item = dataManager.GetItemById(Item.ItemType.SupportItem, id);
            inventory.AddItem(item, 1);
            TreasureBoxTable[TreasureBoxId].accessAble = false;
            return item.ItemName;
        }
        else if (id <= 30000)
        {
            Item item = dataManager.GetItemById(Item.ItemType.Accessory, id);
            inventory.AddItem(item, 1);
            TreasureBoxTable[TreasureBoxId].accessAble = false;
            return item.ItemName;
        }
        else if (id <= 40000)
        {
            Item item = dataManager.GetItemById(Item.ItemType.Weapon, id);
            inventory.AddItem(item, 1);
            TreasureBoxTable[TreasureBoxId].accessAble = false;
            return item.ItemName;
        }
        else
        {
            TreasureBoxTable[TreasureBoxId].accessAble = false;
            return "null";
        }
    }
    ///<summary>
    ///Start時に読み込んだTreasureBoxTable内の要素TreasureBoxData
    ///その中にある、アクセス可能を示すAccessableをセーブデータから復元するメソッド
    ///</summary>
    public void LoadFromSaveData(int[] BoxId,bool[] AccessAble)
    {
        for (int i = 0; i < Mathf.Min(BoxId.Length,AccessAble.Length); i++)
        {
            TreasureBoxTable[BoxId[i]].accessAble = AccessAble[i];
        }
    }
}