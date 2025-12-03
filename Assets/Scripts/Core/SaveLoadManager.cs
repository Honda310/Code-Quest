using UnityEngine;
using System.IO;
using System.Collections.Generic;
using static UnityEditor.Progress;

/// <summary>
/// 【セーブ・ロード】
/// ゲームデータをJSON形式でファイルに保存・読み込みします。
/// </summary>
public class SaveLoadManager : MonoBehaviour
{
    // 保存するデータの形を定義するクラス
    [System.Serializable]
    public class SaveData
    {
        public string saveDate;         // セーブ日時
        public int playerHP;            // プレイヤーHP
        public int netoHP;              // ネトHP
        public string currentMapScene;  // 現在のマップ名
        public List<string> inventoryIDs; // 持っているアイテムのIDリスト
        public List<int> inventoryCounts; // 持っているアイテムの個数リスト
    }

    /// <summary>
    /// ゲームをセーブします
    /// </summary>
    /// <param name="slotId">0はオートセーブ、1以降は手動セーブ</param>
    public void SaveGame(Player p, Neto n, Inventory inv, int slotId)
    {
        SaveData data = new SaveData();

        // データの詰め込み
        data.saveDate = System.DateTime.Now.ToString();
        data.playerHP = p.CurrentHP;
        data.netoHP = n.CurrentHP;

        // インベントリの保存（ループで処理）
        data.inventoryIDs = new List<string>();
        data.inventoryCounts = new List<int>();

        // Inventoryクラスからアイテムリストを取得
        List<CarryItem> items = inv.GetItems();
        foreach (CarryItem item in items)
        {
            data.inventoryIDs.Add(item.item.ItemID);
            data.inventoryCounts.Add(item.quantity);
        }

        // JSON文字列に変換
        string json = JsonUtility.ToJson(data);

        // ファイル名を決定
        string fileName;
        if (slotId == 0) fileName = "autosave.json";
        else fileName = "save" + slotId + ".json";

        // パスを結合して書き込み
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);

        Debug.Log("セーブ完了: " + path);
    }

    /// <summary>
    /// ゲームをロードします
    /// </summary>
    public void LoadGame(Player p, Neto n, Inventory inv, int slotId)
    {
        string fileName;
        if (slotId == 0) fileName = "autosave.json";
        else fileName = "save" + slotId + ".json";

        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // ステータスを復元
            p.CurrentHP = data.playerHP;
            n.CurrentHP = data.netoHP;

            // インベントリを復元
            inv.Clear();
            for (int i = 0; i < data.inventoryIDs.Count; i++)
            {
                string id = data.inventoryIDs[i];
                int count = data.inventoryCounts[i];

                // IDからアイテムデータを検索
                Item itemObj = GameManager.Instance.dataManager.GetItemById(id);

                // アイテムが存在し、かつサポートアイテムであれば追加
                if (itemObj != null && itemObj is SupportItem)
                {
                    inv.AddItem((SupportItem)itemObj, count);
                }
            }
            Debug.Log("ロード完了");
        }
        else
        {
            Debug.LogWarning("セーブデータがありません");
        }
    }

    /// <summary>
    /// オートセーブを削除します
    /// </summary>
    public void DeleteAutoSave()
    {
        string path = Path.Combine(Application.persistentDataPath, "autosave.json");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}