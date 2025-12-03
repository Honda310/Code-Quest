using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 【CSV読込ヘルパー】
/// ResourcesフォルダにあるCSVファイルを読み込み、文字列の配列リストに変換します。
/// </summary>
public static class CSVReader
{
    public static List<string[]> Read(string filePath)
    {
        List<string[]> list = new List<string[]>();

        // Resourcesフォルダからテキストとして読み込む
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);

        if (csvFile != null)
        {
            // 文字列を行ごとに読み込むリーダーを作成
            StringReader reader = new StringReader(csvFile.text);

            // ファイルの最後まで繰り返す
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();

                // 空行でなければカンマで区切ってリストに追加
                if (!string.IsNullOrWhiteSpace(line))
                {
                    list.Add(line.Split(','));
                }
            }
        }
        else
        {
            Debug.LogWarning($"CSVファイルが見つかりません: {filePath}");
        }
        return list;
    }
}