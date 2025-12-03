using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 【CSV解析ヘルパー】
/// 読み込んだ文字列データを、ゲームで使える型（int, bool, Enumなど）に安全に変換するクラスです。
/// データが空だったり不正だったりしても、エラーで停止せずに初期値を返します。
/// </summary>
public static class CSVParser
{
    // --- ファイル読み込み ---

    /// <summary>
    /// ResourcesフォルダからCSVを読み込み、行ごとのリストにして返します。
    /// </summary>
    public static List<string[]> Read(string filePath)
    {
        var list = new List<string[]>();
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);

        if (csvFile != null)
        {
            StringReader reader = new StringReader(csvFile.text);
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                // 空行は無視、カンマで区切る
                if (!string.IsNullOrWhiteSpace(line))
                {
                    list.Add(line.Split(','));
                }
            }
        }
        else
        {
            Debug.LogWarning($"[CSVParser] ファイルが見つかりません: {filePath}");
        }
        return list;
    }

    // --- 型変換メソッド ---

    /// <summary>
    /// 文字列を整数(int)に変換します。失敗したら0を返します。
    /// </summary>
    public static int ParseInt(string value, int defaultValue = 0)
    {
        if (string.IsNullOrEmpty(value)) return defaultValue;
        // TryParseを使うことで、変換失敗時もエラーを出さずに処理を続行できます
        return int.TryParse(value, out int result) ? result : defaultValue;
    }

    /// <summary>
    /// 文字列を小数(float)に変換します。
    /// </summary>
    public static float ParseFloat(string value, float defaultValue = 0f)
    {
        if (string.IsNullOrEmpty(value)) return defaultValue;
        return float.TryParse(value, out float result) ? result : defaultValue;
    }

    /// <summary>
    /// 文字列をEnum（列挙型）に変換します。
    /// 例: "Variable" -> QuestCategory.Variable
    /// </summary>
    public static T ParseEnum<T>(string value, T defaultValue) where T : struct
    {
        if (string.IsNullOrEmpty(value)) return defaultValue;
        return Enum.TryParse(value, true, out T result) ? result : defaultValue;
    }

    /// <summary>
    /// 区切り文字でつながった文字列を、Enumのリストに変換します。
    /// 例: "Variable/Loop" -> { Variable, Loop }
    /// </summary>
    public static List<T> ParseEnumList<T>(string value, char separator = '/') where T : struct
    {
        var list = new List<T>();
        if (string.IsNullOrEmpty(value)) return list;

        string[] items = value.Split(separator);
        foreach (var item in items)
        {
            if (Enum.TryParse(item.Trim(), true, out T result))
            {
                list.Add(result);
            }
        }
        return list;
    }
}