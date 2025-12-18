using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 【ショップ機能】
/// アイテムの物々交換を行うクラスです。
/// </summary>
public class ShopManager : MonoBehaviour
{
    // 交換リスト
    private Dictionary<string, string> exchangeTable = new Dictionary<string, string>();

    public void OpenShop()
    {
        GameManager.Instance.uiManager.ToggleShop(true);
    }

    public void CloseShop()
    {
        GameManager.Instance.uiManager.ToggleShop(false);
    }

    public void Exchange(int payItemID, int getItemID)
    {
        Inventory inv = GameManager.Instance.inventory;

        if (inv.HasItem(payItemID))
        {
            inv.RemoveItem(payItemID, 1);
            // 本来はDataManagerからアイテムを取得して追加
            // inv.AddItem(...);
            Debug.Log("交換成立");
        }
        else
        {
            Debug.Log("アイテムが足りません");
        }
    }
}