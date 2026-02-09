using UnityEngine;
using System.IO;
using System.Collections.Generic;

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
        public string saveDate;
        public Player player;
        public Neto neto;
        public string currentMapName;
        public Vector2 charavector;
        //public 
        public List<int> inventoryIDs;
        public List<int> inventoryCounts;
    }


    /// <summary>
    /// ゲームをセーブします
    /// </summary>
    /// <param name="slotId">0はオートセーブ、1,2は手動セーブ</param>
    public void SaveGame(Player p, Neto n, Inventory inv, int slotId)
    {
        SaveData data = new SaveData();

        // データの詰め込み
        data.saveDate = System.DateTime.Now.ToString();
        data.player = p;
        data.neto = n;
        data.inventoryIDs = new List<int>();
        data.inventoryCounts = new List<int>();
        List<CarryItem> items = inv.GetItems();
        foreach (CarryItem item in items)
        {
            data.inventoryIDs.Add(item.item.ItemID);
            data.inventoryCounts.Add(item.quantity);
        }
        string json = JsonUtility.ToJson(data);
        string fileName;
        if (slotId == 0)
        {
            fileName = "autosave.json";
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, json);
            Debug.Log("セーブ完了: " + path);
        }
        else if(slotId == 1 || slotId==2)
        {
            fileName = "save" + slotId + ".json";
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, json);
            Debug.Log("セーブ完了: " + path);
        }
        else
        {
            Debug.Log("保存先が違うことによるエラーです。SavePointManagerを確認してください。");
        }
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
            p = data.player;
            n = data.neto;

            // インベントリを復元
            inv.Clear();
            for (int i = 0; i < data.inventoryIDs.Count; i++)
            {
                int id = data.inventoryIDs[i];
                int count = data.inventoryCounts[i];

                // IDからアイテムデータを検索
                Item itemObj = GameManager.Instance.dataManager.GetItemById(Item.ItemType.SupportItem,id);

                // アイテムが存在し、かつサポートアイテムであれば追加
                if (itemObj != null && itemObj is SupportItem)
                {
                    inv.AddItem((SupportItem)itemObj, count);
                }
            }
        }
        else
        {

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