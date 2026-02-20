using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 【データ管理】
/// CSVデータを読み込み、メモリ上の辞書に格納するクラス。
/// 敵データの画像ファイル名読み込みに対応。
/// </summary>
public class DataManager : MonoBehaviour
{
    public Dictionary<int, Weapon> WeaponMaster = new Dictionary<int, Weapon>();
    public Dictionary<int, Accessory> AccessoryMaster = new Dictionary<int, Accessory>();
    public Dictionary<int, SupportItem> SupportItemMaster = new Dictionary<int, SupportItem>();
    public Dictionary<int, EnemyData> EnemyMaster = new Dictionary<int, EnemyData>();

    public void LoadAllData()
    {
        LoadWeapons("Data/Weapon");
        LoadAccessories("Data/Accessory");
        LoadSupportItems("Data/SupportItem");
        LoadEnemies("Data/Enemy");
        LoadTreasureBoxes("Data/TreasureBox");
        
        Debug.Log("全データのロードが完了しました。");
    }

    private void LoadWeapons(string path)
    {
        var csv = CSVReader.Read(path);
        foreach (var line in csv)
        {
            if (line.Length == 7)
            {
                int id = int.Parse(line[0]);
                Weapon w = new Weapon(id, line[1], int.Parse(line[2]), int.Parse(line[3]), int.Parse(line[4]), Enum.Parse<Item.ItemType>(line[5]), line[6]);
                if (!WeaponMaster.ContainsKey(id)) WeaponMaster.Add(id, w);
            }
        }
        Debug.Log($"武器マスタ: {WeaponMaster.Count}件 ロード完了");
    }

    private void LoadAccessories(string path)
    {
        var csv = CSVReader.Read(path);
        foreach (var line in csv)
        {
            if (line.Length == 6)
            {
                int id = int.Parse(line[0]);
                Accessory a = new Accessory(id, line[1], int.Parse(line[2]), int.Parse(line[3]), Enum.Parse<Item.ItemType>(line[4]), line[5]);
                if (!AccessoryMaster.ContainsKey(id)) AccessoryMaster.Add(id, a);
            }
        }
        Debug.Log($"アクセサリマスタ: {AccessoryMaster.Count}件 ロード完了");
    }

    private void LoadSupportItems(string path)
    {
        var csv = CSVReader.Read(path);
        foreach (var line in csv)
        {
            if (line.Length == 7)
            {
                int id = int.Parse(line[0]);
                SupportItem s = new SupportItem(id, line[1], int.Parse(line[2]), int.Parse(line[3]), int.Parse(line[4]), Enum.Parse<Item.ItemType>(line[5]), line[6]);
                if (!SupportItemMaster.ContainsKey(id)) SupportItemMaster.Add(id, s);
            }
        }
        Debug.Log($"サポートアイテムマスタ: {SupportItemMaster.Count}件 ロード完了");
    }

    private void LoadEnemies(string path)
    {
        List<string[]> csv = CSVParser.Read(path);

        for (int i = 1; i < csv.Count; i++)
        {
            string[] line = csv[i];
            if (line.Length < 7) continue;

            int id = CSVParser.ParseInt(line[0]);
            string name = line[1];
            int maxDp = CSVParser.ParseInt(line[2]);
            int atk = CSVParser.ParseInt(line[3]);

            List<QuestCategory> categories = CSVParser.ParseEnumList<QuestCategory>(line[4], '/');

            string imageFileName = line[5];
            int Exp = CSVParser.ParseInt(line[6]);

            EnemyData data = new EnemyData(id, name, maxDp, atk, categories, imageFileName,Exp);

            if (!EnemyMaster.ContainsKey(id))
            {
                EnemyMaster.Add(id, data);
            }
            GameManager.Instance.enemyList.EnemyRegist(id);
        }
        Debug.Log($"敵マスタ: {EnemyMaster.Count}件 ロード完了");
    }
    private void LoadTreasureBoxes(string path)
    {
        List<string[]> csv = CSVParser.Read(path);
        for (int i = 1; i < csv.Count; i++)
        {
            string[] line = csv[i];
            if (line.Length < 2) continue;
            int id = CSVParser.ParseInt(line[0]);
            int itemid = CSVParser.ParseInt(line[1]);
            GameManager.Instance.treasureBoxList.CreateTreasureBox(id, itemid);
        }
        
    }
	public Item GetItemById(Item.ItemType type, int id)
	{
	    switch (type)
	    {
	        case Item.ItemType.Weapon:
	            return WeaponMaster.TryGetValue(id, out var w) ? w : null;

	        case Item.ItemType.Accessory:
	            return AccessoryMaster.TryGetValue(id, out var a) ? a : null;

	        case Item.ItemType.SupportItem:
	            return SupportItemMaster.TryGetValue(id, out var s) ? s : null;
	    }
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
    public string ImageFileName;
    public int Exp;
    public EnemyData(int id, string name, int dp, int atk, List<QuestCategory> categories, string imgName, int exp)
    {
        ID = id;
        Name = name;
        MaxDP = dp;
        Atk = atk;
        Categories = categories;
        ImageFileName = imgName;
        Exp = exp;
    }
}