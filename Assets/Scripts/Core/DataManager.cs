using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 【データ管理】
/// CSVデータを読み込み、メモリ上の辞書に格納するクラス。
/// 敵データの画像ファイル名読み込みに対応。
/// </summary>
public class DataManager : MonoBehaviour
{
    public Dictionary<string, Weapon> WeaponMaster = new Dictionary<string, Weapon>();
    public Dictionary<string, Accessory> AccessoryMaster = new Dictionary<string, Accessory>();
    public Dictionary<string, SupportItem> SupportItemMaster = new Dictionary<string, SupportItem>();

    // 敵データ検索用辞書
    public Dictionary<int, EnemyData> EnemyMaster = new Dictionary<int, EnemyData>();

    public void LoadAllData()
    {
        LoadWeapons("Data/Weapon");
        LoadAccessories("Data/Accessory");
        LoadSupportItems("Data/SupportItem");
        LoadEnemies("Data/Enemy"); // 敵データロード

        Debug.Log("全データのロードが完了しました。");
    }

    // --- (Weapon, Accessory, SupportItem のロード処理は省略せず記述する場合、以前と同様) ---
    // ここでは長くなるため省略しますが、以前のコードのままでOKです。
    // 今回重要な Enemy のロード処理のみ詳細に記述します。

    private void LoadWeapons(string path)
    {
        var csv = CSVReader.Read(path);
        foreach (var line in csv)
        {
            if (line.Length < 6) continue;
            Weapon w = new Weapon(line[0], line[1], int.Parse(line[2]), int.Parse(line[3]), int.Parse(line[4]), line[5]);
            if (!WeaponMaster.ContainsKey(w.ItemID)) WeaponMaster.Add(w.ItemID, w);
        }
    }

    private void LoadAccessories(string path)
    {
        var csv = CSVReader.Read(path);
        foreach (var line in csv)
        {
            if (line.Length < 5) continue;
            Accessory a = new Accessory(line[0], line[1], int.Parse(line[2]), int.Parse(line[3]), line[4]);
            if (!AccessoryMaster.ContainsKey(a.ItemID)) AccessoryMaster.Add(a.ItemID, a);
        }
    }

    private void LoadSupportItems(string path)
    {
        var csv = CSVReader.Read(path);
        foreach (var line in csv)
        {
            if (line.Length < 6) continue;
            SupportItem s = new SupportItem(line[0], line[1], int.Parse(line[2]), int.Parse(line[3]), int.Parse(line[4]), line[5]);
            if (!SupportItemMaster.ContainsKey(s.ItemID)) SupportItemMaster.Add(s.ItemID, s);
        }
    }

    // --------------------------------------------------------
    // ★修正箇所: 敵データのロード（画像ファイル名対応）
    // --------------------------------------------------------
    private void LoadEnemies(string path)
    {
        List<string[]> csv = CSVParser.Read(path); // CSVParserを使用

        for (int i = 1; i < csv.Count; i++)
        {
            string[] line = csv[i];

            // CSV列: [0]ID, [1]Name, [2]MaxDP, [3]Atk, [4]Categories, [5]ImageFileName
            // 必須列が足りない場合はスキップ
            if (line.Length < 6) continue;

            int id = CSVParser.ParseInt(line[0]);
            string name = line[1];
            int maxDp = CSVParser.ParseInt(line[2]);
            int atk = CSVParser.ParseInt(line[3]);

            // カテゴリリスト変換 (例: "Variable/Loop")
            List<QuestCategory> categories = CSVParser.ParseEnumList<QuestCategory>(line[4], '/');

            // 画像ファイル名取得
            string imageFileName = line[5];

            // データ生成
            EnemyData data = new EnemyData(id, name, maxDp, atk, categories, imageFileName);

            if (!EnemyMaster.ContainsKey(id))
            {
                EnemyMaster.Add(id, data);
            }
        }
        Debug.Log($"敵マスタ: {EnemyMaster.Count}件 ロード完了");
    }

    public Item GetItemById(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        char type = id[0];
        if (type == 'W' && WeaponMaster.ContainsKey(id)) return WeaponMaster[id];
        if (type == 'A' && AccessoryMaster.ContainsKey(id)) return AccessoryMaster[id];
        if (type == 'S' && SupportItemMaster.ContainsKey(id)) return SupportItemMaster[id];
        return null;
    }

    // 敵データ取得用
    public EnemyData GetEnemyById(int id)
    {
        if (EnemyMaster.ContainsKey(id)) return EnemyMaster[id];
        return null;
    }
}

/// <summary>
/// 敵のマスタデータ保持用クラス
/// </summary>
public class EnemyData
{
    public int ID;
    public string Name;
    public int MaxDP;
    public int Atk;
    public List<QuestCategory> Categories;
    public string ImageFileName; // ★画像ファイル名

    public EnemyData(int id, string name, int dp, int atk, List<QuestCategory> cats, string imgName)
    {
        ID = id; Name = name; MaxDP = dp; Atk = atk; Categories = cats; ImageFileName = imgName;
    }
}