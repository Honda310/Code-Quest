using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, List<QuestData>> categorizedQuests = new Dictionary<string, List<QuestData>>();
    private Queue<QuestData> currentDeck = new Queue<QuestData>();

    public void LoadQuests()
    {
        categorizedQuests.Clear();
        // ファイル名とカテゴリを一致させて読み込み
        LoadFromSingleCSV("Data/SelectQuestion/Variable_AdditionAndSubtraction", QuestCategory.Variable_AdditionAndSubtraction);
        LoadFromSingleCSV("Data/SelectQuestion/Variable_IncrementAndCompoundAssignmentPrecedence", QuestCategory.Variable_IncrementAndCompoundAssignmentPrecedence);
        LoadFromSingleCSV("Data/SelectQuestion/Variable_MultiplicationAndDivisionAndRemainder", QuestCategory.Variable_MultiplicationAndDivisionAndRemainder);
        LoadFromSingleCSV("Data/SelectQuestion/IF_BasicComparison", QuestCategory.IF_BasicComparison);
        LoadFromSingleCSV("Data/SelectQuestion/IF_ElseIf", QuestCategory.IF_ElseIf);
        LoadFromSingleCSV("Data/SelectQuestion/IF_LogicalOperator", QuestCategory.IF_LogicalOperator);
        Debug.Log("[QuestManager] ロード完了");
    }

    private void LoadFromSingleCSV(string path, QuestCategory category)
    {
        List<string[]> csv = CSVParser.Read(path);
        string catName = category.ToString();
        List<QuestData> list = new List<QuestData>();

        // ヘッダー行(0)をスキップして1行目から
        for (int i = 1; i < csv.Count; i++)
        {
            string[] cols = csv[i];
            cols[1] = cols[1].Replace("\\n", "\n");
            // ★修正：列数で形式を判別
            if (cols.Length >= 7)
            {
                cols[1] = cols[1].Replace("\\n", "\n");
                // [4択形式] ID, 問題, A, B, C, D, 答え
                string[] opts = new string[] { cols[2], cols[3], cols[4], cols[5] };
                list.Add(new QuestData(cols[0], category, cols[1], opts, cols[6]));
            }
            else if (cols.Length >= 3)
            {
                // [記述形式] ID, 問題, 答え
                list.Add(new QuestData(cols[0], category, cols[1], cols[2]));
            }
        }
        Debug.Log(list.Count);
        if (list.Count > 0)
        {
            if (categorizedQuests.ContainsKey(catName)) categorizedQuests[catName].AddRange(list);
            else categorizedQuests.Add(catName, list);
        }
    }

    public void CreateDeck(List<QuestCategory> categories)
    {
        currentDeck.Clear();
        List<QuestData> pool = new List<QuestData>();

        foreach (var cat in categories)
        {
            string name = cat.ToString();
            if (categorizedQuests.ContainsKey(name)) pool.AddRange(categorizedQuests[name]);
        }

        // シャッフル
        for (int i = 0; i < pool.Count; i++)
        {
            QuestData temp = pool[i];
            int r = UnityEngine.Random.Range(i, pool.Count);
            pool[i] = pool[r];
            pool[r] = temp;
        }

        foreach (var q in pool) currentDeck.Enqueue(q);
    }

    public QuestData GetNextQuestion()
    {
        return currentDeck.Count > 0 ? currentDeck.Dequeue() : null;
    }
}