/// <summary>
/// 1問分の問題データを保持するクラスです。
/// </summary>
[System.Serializable]
public class QuestData
{
    public string ID;
    public QuestCategory Category;
    public string QuestionText;
    public string CorrectAnswer; // 完全一致用の正解
    public string[] Keywords;    // 部分一致判定用のキーワード配列

    public QuestData(string id, QuestCategory category, string text, string answer)
    {
        ID = id;
        Category = category;
        QuestionText = text;
        CorrectAnswer = answer;

        // 正解コードを分割してキーワードリストを作る
        Keywords = answer.Split(new char[] { ' ', ';', '(', ')', '{', '}' }, System.StringSplitOptions.RemoveEmptyEntries);
    }
}