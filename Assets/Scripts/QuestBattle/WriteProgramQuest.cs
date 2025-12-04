using UnityEngine;

public class WriteProgramQuest : MonoBehaviour
{
    /// <summary>
    /// 入力された答えが正しいか判定する
    /// </summary>
    public bool CheckAnswer(string input, QuestData data)
    {
        // 入力値の正規化（空白・改行削除）
        string normInput = input.Replace(" ", "").Replace("\n", "");
        string normAns = data.CorrectAnswer.Replace(" ", "").Replace("\n", "");

        // 1. 完全一致チェック (4択の数字 "1"=="1" や、短いコードの一致)
        if (normInput == normAns) return true;

        // 2. キーワード判定 (記述式の場合のみ)
        // ★修正: data.Keywords が null の場合はこの処理をスキップして false を返す
        if (data.Keywords != null && data.Keywords.Length > 0)
        {
            foreach (string key in data.Keywords)
            {
                // キーワードが含まれていなければ不正解
                if (!normInput.Contains(key)) return false;
            }
            // 全キーワードが含まれていれば正解とみなす
            return true;
        }

        return false;
    }
}