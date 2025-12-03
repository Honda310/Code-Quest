using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 【問題管理】
/// 全問題を読み込み、敵に合わせて出題デッキを作成するクラスです。
/// </summary>
public class QuestManager : MonoBehaviour
{
    private List<QuestData> allQuests = new List<QuestData>();
    private Queue<QuestData> currentDeck = new Queue<QuestData>();

    // 初期化
    public void LoadQuests()
    {
        allQuests.Clear();
        // CSVファイルを読み込む（パスはUnityのResourcesフォルダ構成に合わせる）
        LoadFromCSV("Data/Variable", QuestCategory.Variable);
        LoadFromCSV("Data/IF", QuestCategory.If);
        LoadFromCSV("Data/Loop", QuestCategory.Loop);
        LoadFromCSV("Data/Array", QuestCategory.Array);

        Debug.Log($"クエストロード完了: 全 {allQuests.Count} 問");
    }

    private void LoadFromCSV(string path, QuestCategory category)
    {
        List<string[]> csvData = CSVReader.Read(path);
        foreach (string[] line in csvData)
        {
            if (line.Length < 3) continue;
            // ID, 問題文, 正解
            allQuests.Add(new QuestData(line[0], category, line[1], line[2]));
        }
    }

    /// <summary>
    /// 指定されたカテゴリの問題を集め、シャッフルしてデッキを作成します
    /// </summary>
    public void CreateDeck(List<QuestCategory> categories)
    {
        currentDeck.Clear();
        List<QuestData> filteredList = new List<QuestData>();

        // 1. カテゴリに合致する問題を抽出
        foreach (QuestData q in allQuests)
        {
            if (categories.Contains(q.Category))
            {
                filteredList.Add(q);
            }
        }

        // 2. シャッフル (フィッシャー–イェーツのシャッフル)
        for (int i = 0; i < filteredList.Count; i++)
        {
            QuestData temp = filteredList[i];
            int randomIndex = Random.Range(i, filteredList.Count);
            filteredList[i] = filteredList[randomIndex];
            filteredList[randomIndex] = temp;
        }

        // 3. デッキ（キュー）に追加
        foreach (QuestData q in filteredList)
        {
            currentDeck.Enqueue(q);
        }
    }

    // 次の問題を取り出す
    public QuestData GetNextQuestion()
    {
        if (currentDeck.Count > 0)
        {
            return currentDeck.Dequeue();
        }
        return null;
    }
}