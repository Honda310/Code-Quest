using UnityEngine;

[System.Serializable]
public class QuestData
{
    public string ID;
    public QuestCategory Category;
    public string QuestionText;
    public string CorrectAnswer; 
    public string[] Options;
    public string[] Keywords;

    public QuestData(string id, QuestCategory cat, string q, string a)
    {
        ID = id;
        Category = cat;
        QuestionText = q;
        CorrectAnswer = a;
    }
    public QuestData(string id, QuestCategory cat, string q, string[] opts, string a)
    {
        ID = id;
        Category = cat;
        QuestionText = q;
        Options = opts;
        CorrectAnswer = a; 
        Keywords = null;   // 4択なのでキーワード判定は不要（nullにする）
    }

    public bool IsMultipleChoice => Options != null && Options.Length > 0;
}