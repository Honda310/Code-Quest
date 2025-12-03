using UnityEngine;

/// <summary>
/// 【記述判定】
/// 入力されたコードが正解かどうかを文字列解析で判定します。
/// </summary>
public class WriteProgramQuest : MonoBehaviour
{
    public bool CheckAnswer(string inputCode, QuestData questData)
    {
        // 空白や改行を削除して比較しやすくする
        string normalizedInput = inputCode.Replace(" ", "").Replace("\n", "");
        string normalizedAnswer = questData.CorrectAnswer.Replace(" ", "").Replace("\n", "");

        // 完全一致チェック
        if (normalizedInput == normalizedAnswer)
        {
            return true;
        }

        // キーワード含有チェック（救済措置）
        // 必須キーワードが全て含まれていれば正解とする
        foreach (string key in questData.Keywords)
        {
            if (!normalizedInput.Contains(key))
            {
                return false;
            }
        }
        return true;
    }
}