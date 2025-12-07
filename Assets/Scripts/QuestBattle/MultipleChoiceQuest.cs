using UnityEngine;

/// <summary>
/// 【4択判定】
/// プレイヤーが選んだ選択肢番号と、データの正解番号を比較します。
/// </summary>
public class MultipleChoiceQuest : MonoBehaviour
{
    /// <summary>
    /// 正誤判定を行う
    /// </summary>
    /// <param name="selectedNumber">プレイヤーが押したボタンの番号 (1～4)</param>
    /// <param name="data">現在の問題データ</param>
    /// <returns>正解ならtrue</returns>
    public bool CheckAnswer(string selectedNumber, QuestData data)
    {
        // データが4択用かチェック
        if (!data.IsMultipleChoice) return false;

        // CSVの正解列(CorrectAnswer)は "1", "2" などの文字列として入っているため、
        // 選択された数値(int)を文字列(String)に変換して比較します。
        return selectedNumber == data.CorrectAnswer.Trim();
    }
}