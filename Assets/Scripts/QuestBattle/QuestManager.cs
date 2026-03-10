using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, List<QuestData>> categorizedQuests = new Dictionary<string, List<QuestData>>();
    List<CodingQuestData> codingQuestList = new List<CodingQuestData>();
    private Queue<QuestData> currentDeckNormal = new Queue<QuestData>();
    private Queue<QuestData> currentDeckHard = new Queue<QuestData>();

    public void LoadQuests()
    {
        categorizedQuests.Clear();
        // ファイル名とカテゴリを一致させて読み込み
        LoadFromSingleCSV("Data/SelectQuestion/Variable_AdditionAndSubtraction", QuestCategory.Variable_AdditionAndSubtraction);
        LoadFromSingleCSV("Data/SelectQuestion/Variable_IncrementAndCompoundAssignmentPrecedence", QuestCategory.Variable_IncrementAndCompoundAssignmentPrecedence);
        LoadFromSingleCSV("Data/SelectQuestion/Variable_MultiplicationAndDivisionAndRemainder", QuestCategory.Variable_MultiplicationAndDivisionAndRemainder);
        LoadFromSingleCSV("Data/SelectQuestion/Variable_All", QuestCategory.Variable_All);
        LoadFromSingleCSV("Data/SelectQuestion/IF_BasicComparison", QuestCategory.IF_BasicComparison);
        LoadFromSingleCSV("Data/SelectQuestion/IF_ElseIf", QuestCategory.IF_ElseIf);
        LoadFromSingleCSV("Data/SelectQuestion/IF_LogicalOperator", QuestCategory.IF_LogicalOperator);
        LoadFromSingleCSV("Data/SelectQuestion/IF_All", QuestCategory.IF_All);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_IF", QuestCategory.IF_BasicComparisonHard);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_IF", QuestCategory.IF_ElseIfHard);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_IF", QuestCategory.IF_LogicalOperatorHard);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_IF", QuestCategory.IF_AllHard);
        LoadFromSingleCSV("Data/SelectQuestion/ForWhile_Basic", QuestCategory.ForWhile_Basic);
        LoadFromSingleCSV("Data/SelectQuestion/ForWhile_Nest", QuestCategory.ForWhile_Nest);
        LoadFromSingleCSV("Data/SelectQuestion/ForWhile_WhileAndDoWhile", QuestCategory.ForWhile_WhileAndDoWhile);
        LoadFromSingleCSV("Data/SelectQuestion/ForWhile_All", QuestCategory.ForWhile_All);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_ForWhile", QuestCategory.ForWhile_BasicHard);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_ForWhile", QuestCategory.ForWhile_NestHard);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_ForWhile", QuestCategory.ForWhile_WhileAndDoWhileHard);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_ForWhile", QuestCategory.ForWhile_AllHard);
        LoadFromSingleCSV("Data/SelectQuestion/Array_Access_Index", QuestCategory.Array_Access_Index);
        LoadFromSingleCSV("Data/SelectQuestion/Array_Decl_Init", QuestCategory.Array_Decl_Init);
        LoadFromSingleCSV("Data/SelectQuestion/Array_Length_Loop", QuestCategory.Array_Length_Loop);
        LoadFromSingleCSV("Data/SelectQuestion/Array_Types_Values", QuestCategory.Array_Types_Values);
        LoadFromSingleCSV("Data/SelectQuestion/Array_All", QuestCategory.Array_All);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_Array", QuestCategory.Array_Access_IndexHard);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_Array", QuestCategory.Array_Decl_InitHard);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_Array", QuestCategory.Array_Length_LoopHard);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_Array", QuestCategory.Array_Types_ValuesHard);
        LoadFromSingleCSV("Data/FillBlankQuestion/FillIn_Array", QuestCategory.Array_AllHard);
        LoadFromCodingCSV("Data/CodingQuest/CodingQuestAll");
        Debug.Log("[QuestManager] ロード完了");
    }
    private void LoadFromSingleCSV(string path, QuestCategory category)
    {
        List<string[]> csv = CSVParser.Read(path);
        string catName = category.ToString();
        List<QuestData> list = new List<QuestData>();

        for (int i = 1; i < csv.Count; i++)
        {
            string[] cols = csv[i];
            cols[1] = cols[1].Replace("\\n", "\n");
            cols[1] = cols[1].Replace("■",",");
            if (cols.Length == 7)
            {
                // [4択形式] ID, 問題, A, B, C, D, 答え
                string[] opts = new string[] { cols[2], cols[3], cols[4], cols[5] };
                list.Add(new QuestData(cols[0], category, cols[1], opts, cols[6]));
            }
            else if (cols.Length == 3)
            {
                // [記述形式] ID, 問題, 答え
                list.Add(new QuestData(cols[0], category, cols[1], cols[2]));
            }
        }
        if (list.Count > 0)
        {
            if (categorizedQuests.ContainsKey(catName)) categorizedQuests[catName].AddRange(list);
            else categorizedQuests.Add(catName, list);
        }
    }
    private void LoadFromCodingCSV(string path)
    {
        List<string[]> csv = CSVParser.Read(path);

        for (int i = 1; i < csv.Count; i++)
        {
            string[] cols = csv[i];
            if (cols.Length == 7)
            {
                int id = int.Parse(cols[0]);
                cols[1] = cols[1].Replace("\\n", "\n");
                cols[1] = cols[1].Replace("■", ",");
                cols[5] = cols[5].Replace("\\n", "\n");
                cols[6] = cols[6].Replace("\\n", "\n");
                string[] answers = { cols[2], cols[3], cols[4] };
                codingQuestList.Add(new CodingQuestData(id, cols[1], answers, cols[5], cols[6]));
            }
        }
    }
    public void CreateDeckNormal(QuestCategory categories)
    {
        currentDeckNormal.Clear();
        List<QuestData> pool = new List<QuestData>();

        string name = categories.ToString();
        if (categorizedQuests.ContainsKey(name)) pool.AddRange(categorizedQuests[name]);

        // シャッフル
        for (int i = 0; i < pool.Count; i++)
        {
            QuestData temp = pool[i];
            int r = Random.Range(i, pool.Count);
            pool[i] = pool[r];
            pool[r] = temp;
        }
        foreach (var q in pool) currentDeckNormal.Enqueue(q);
    }
    public void CreateDeckHard(QuestCategory categories)
    {
        currentDeckHard.Clear();
        List<QuestData> pool = new List<QuestData>();

        string name = categories.ToString();
        if (categorizedQuests.ContainsKey(name)) pool.AddRange(categorizedQuests[name+"Hard"]);

        // シャッフル
        for (int i = 0; i < pool.Count; i++)
        {
            QuestData temp = pool[i];
            int r = Random.Range(i, pool.Count);
            pool[i] = pool[r];
            pool[r] = temp;
        }
        foreach (var q in pool) currentDeckHard.Enqueue(q);
    }
    public QuestData GetNextQuestionNormal()
    {
        return currentDeckNormal.Count > 0 ? currentDeckNormal.Dequeue() : null;
    }
    public QuestData GetNextQuestionHard()
    {
        return currentDeckHard.Count > 0 ? currentDeckHard.Dequeue() : null;
    }
    public CodingQuestData GetCodingQuestion(int value)
    {
        Debug.Log($"value: {value}");
        Debug.Log($"Count: {codingQuestList.Count}");
        return codingQuestList[value];
    }
}