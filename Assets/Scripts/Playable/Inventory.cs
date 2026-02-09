using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 【所持品管理】
/// サポートアイテムの所持数をリストで管理します。
/// </summary>
public class Inventory : MonoBehaviour
{
    // 所持しているアイテムのリスト
    [NonSerialized] public List<CarryItem> items = new List<CarryItem>();

    /// <summary>
    /// アイテムを追加します
    /// </summary>
    public void AddItem(Item item, int amount)
    {
        CarryItem target = items.Find(c => c.item.ItemID == item.ItemID);

        if (target != null)
        {
            target.quantity += amount;
        }
        else
        {
            items.Add(new CarryItem(item, amount));
        }

        Debug.Log($"{item.ItemName} を {amount}個 入手しました。");
    }
    public enum ItemType
	{
	    Weapon,
	    Accessory,
	    Support
	}
    /// <summary>
    /// アイテムを持っているか確認します
    /// </summary>
    public bool HasItem(int itemId)
    {
        return items.Exists(c => c.item.ItemID == itemId && c.quantity > 0);
    }

    public void RemoveItem(int itemId, int amount)
    {
        CarryItem target = items.Find(c => c.item.ItemID == itemId);
        if (target == null) return;

        target.quantity -= amount;
        if (target.quantity <= 0)
        {
            items.Remove(target);
        }
    }

    // 外部からリストを取得するため
    public List<CarryItem> GetItems()
    {
        return items;
    }

    public void Clear()
    {
        items.Clear();
    }
    public List<CarryItem> GetSortedItems()
    {
        return items
            .OrderBy(c => c.item.ItemID)
            .ToList();
    }

    public List<CarryItem> GetItemsByType(Item.ItemType type)
    {
        return items
            .Where(c => c.item.Type == type)
            .OrderBy(c => c.item.ItemID)
            .ToList();
    }
    void Refresh(Item.ItemType? filter = null)
    {
        List<CarryItem> list = filter.HasValue
            ? GetItemsByType(filter.Value)
            : GetSortedItems();
    }
    public void LoadItems(List<CarryItem> list)
    {
        items = list;
    }
}