using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 【所持品管理】
/// サポートアイテムの所持数をリストで管理します。
/// </summary>
public class Inventory : MonoBehaviour
{
    // 所持しているアイテムのリスト
    [SerializeField] private List<CarryItem> items = new List<CarryItem>();

    /// <summary>
    /// アイテムを追加します
    /// </summary>
    public void AddItem(SupportItem item, int amount)
    {
        // すでに持っているか確認する
        CarryItem target = null;
        foreach (CarryItem c in items)
        {
            if (c.item.ItemID == item.ItemID)
            {
                target = c;
                break;
            }
        }

        if (target != null)
        {
            // 持っていれば個数を増やす
            target.quantity += amount;
        }
        else
        {
            // 持っていなければ新しくリストに追加
            items.Add(new CarryItem(item, amount));
        }

        Debug.Log($"{item.ItemName} を {amount}個 入手しました。");
    }

    /// <summary>
    /// アイテムを持っているか確認します
    /// </summary>
    public bool HasItem(string itemId)
    {
        foreach (CarryItem c in items)
        {
            if (c.item.ItemID == itemId && c.quantity > 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// アイテムを消費します
    /// </summary>
    public void RemoveItem(string itemId, int amount)
    {
        CarryItem target = null;
        foreach (CarryItem c in items)
        {
            if (c.item.ItemID == itemId)
            {
                target = c;
                break;
            }
        }

        if (target != null)
        {
            target.quantity -= amount;

            // 個数が0以下になったらリストから削除
            if (target.quantity <= 0)
            {
                items.Remove(target);
            }
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
}