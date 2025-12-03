using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Progress;

/// <summary>
/// 【データ管理】
/// 武器、防具、サポートアイテムのデータをCSVファイルから読み込み、管理します。
/// </summary>
public class DataManager : MonoBehaviour
{
    // IDをキーにしてデータを検索するための辞書
    public Dictionary<string, Weapon> WeaponMaster = new Dictionary<string, Weapon>();
    public Dictionary<string, Accessory> AccessoryMaster = new Dictionary<string, Accessory>();
    public Dictionary<string, SupportItem> SupportItemMaster = new Dictionary<string, SupportItem>(); // ※名前修正済み

    /// <summary>
    /// すべてのデータを読み込むメソッド
    /// </summary>
    public void LoadAllData()
    {
        // Resources/Data フォルダにあるCSVを読み込みます
        LoadWeapons("Data/Weapon");
        LoadAccessories("Data/Accessory");
        LoadSupportItems("Data/SupportItem");

        Debug.Log($"データロード完了: 武器{WeaponMaster.Count}件, 防具{AccessoryMaster.Count}件, 道具{SupportItemMaster.Count}件");
    }

    // 武器データの読み込み
    private void LoadWeapons(string path)
    {
        List<string[]> csvData = CSVReader.Read(path);

        // 1行ずつ処理
        for (int i = 0; i < csvData.Count; i++)
        {
            string[] line = csvData[i];

            // データ列が足りない場合はスキップ
            if (line.Length < 6) continue;

            // CSVのカラムに合わせてデータを生成
            Weapon w = new Weapon(
                line[0],            // ID (例: Wb001)
                line[1],            // 名前
                int.Parse(line[2]), // レアリティ
                int.Parse(line[3]), // 攻撃力
                int.Parse(line[4]), // 時間制限
                line[5]             // 説明文
            );

            // 辞書に登録（重複チェック付き）
            if (!WeaponMaster.ContainsKey(w.ItemID))
            {
                WeaponMaster.Add(w.ItemID, w);
            }
        }
    }

    // アクセサリ（防具）データの読み込み
    private void LoadAccessories(string path)
    {
        List<string[]> csvData = CSVReader.Read(path);

        for (int i = 0; i < csvData.Count; i++)
        {
            string[] line = csvData[i];
            if (line.Length < 5) continue;

            Accessory a = new Accessory(
                line[0],            // ID
                line[1],            // 名前
                int.Parse(line[2]), // レアリティ
                int.Parse(line[3]), // 防御力
                line[4]             // 説明文
            );

            if (!AccessoryMaster.ContainsKey(a.ItemID))
            {
                AccessoryMaster.Add(a.ItemID, a);
            }
        }
    }

    // サポートアイテムデータの読み込み
    private void LoadSupportItems(string path)
    {
        List<string[]> csvData = CSVReader.Read(path);

        for (int i = 0; i < csvData.Count; i++)
        {
            string[] line = csvData[i];
            if (line.Length < 6) continue;

            SupportItem s = new SupportItem(
                line[0],            // ID
                line[1],            // 名前
                int.Parse(line[2]), // レアリティ
                int.Parse(line[3]), // 効果ID
                int.Parse(line[4]), // 効果量
                line[5]             // 説明文
            );

            if (!SupportItemMaster.ContainsKey(s.ItemID))
            {
                SupportItemMaster.Add(s.ItemID, s);
            }
        }
    }

    /// <summary>
    /// IDからアイテムデータを検索して返す便利メソッド
    /// </summary>
    public Item GetItemById(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;

        // IDの先頭文字で種類を判定 (設計書6.1準拠)
        char type = id[0];

        if (type == 'W') // Weapon
        {
            if (WeaponMaster.ContainsKey(id)) return WeaponMaster[id];
        }
        else if (type == 'A') // Accessory
        {
            if (AccessoryMaster.ContainsKey(id)) return AccessoryMaster[id];
        }
        else if (type == 'S') // SupportItem
        {
            if (SupportItemMaster.ContainsKey(id)) return SupportItemMaster[id];
        }

        return null; // 見つからなかった場合
    }
}