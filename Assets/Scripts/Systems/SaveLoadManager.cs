using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
        public List<CarryItem> carryitems;
        public EnemyList enemyList;
        public TreasureBoxList treasureList;
        public List<int> inventoryIDs;
        public List<int> inventoryCounts;
        public string currentMapName;
        public Vector3 charavector;
    }
    private SaveData loadedData;

    /// <summary>
    /// ゲームをセーブします
    /// </summary>
    /// <param name="slotId">0はオートセーブ、1,2は手動セーブ</param>
    public void SaveGame(Player p, Neto n, List<CarryItem> items, int slotId,TreasureBoxList boxList,EnemyList enemies)
    {
        SaveData data = new SaveData();
        data.saveDate = System.DateTime.Now.ToString();     //セーブした日時⓪
        data.player = p;                                    //プレイヤーの情報①
        data.neto = n;                                      //ネトちゃんの情報②
        data.carryitems = items;                            //インベントリ情報③
        data.treasureList = boxList;                        //開封済み宝箱の情報④
        data.enemyList = enemies;                           //討伐済みのエネミー情報⑤
        Scene currentScene = SceneManager.GetActiveScene(); //現在シーンの取得
        data.currentMapName = currentScene.name;            //セーブしたマップ名の情報⑥
        data.charavector = p.transform.position;   //セーブした座標の情報⑦
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
    public void LoadGame(int slotId)
    {
        string fileName;

        if (slotId == 0)
            fileName = "autosave.json";
        else if (slotId == 1 || slotId == 2)
            fileName = "save" + slotId + ".json";
        else
        {
            Debug.LogError("不正なスロットIDです");
            return;
        }

        string path = Path.Combine(Application.persistentDataPath, fileName);


        string json = File.ReadAllText(path);
        loadedData = JsonUtility.FromJson<SaveData>(json);

        Player player = Object.FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.LoadFromSaveData(loadedData.player);
            player.transform.position = loadedData.charavector;
        }
        Neto neto = Object.FindFirstObjectByType<Neto>();
        if (neto != null)
        {
            neto.LoadFromSaveData(loadedData.neto);
        }
        Inventory inventory = Object.FindFirstObjectByType<Inventory>();
        if (inventory != null)
        {
            inventory.LoadItems(loadedData.carryitems);
        }
        TreasureBoxList treasureList = Object.FindFirstObjectByType<TreasureBoxList>();
        if (treasureList != null)
        {
            treasureList.LoadFromSaveData(loadedData.treasureList.TreasureBoxTable);
        }
        EnemyList enemyList = Object.FindFirstObjectByType<EnemyList>();
        if (enemyList != null)
        {
            enemyList.LoadFromSaveData(loadedData.enemyList.enemyDefeated);
        }
        SceneManager.LoadScene(loadedData.currentMapName);
        player.transform.position = loadedData.charavector;
        neto.transform.position = loadedData.charavector;
    }

    /// <summary>
    /// Sceneロード完了後にデータを反映
    /// </summary>
    /// <summary>
    /// オートセーブを削除するメソッド、正常終了時に呼び出す用
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
