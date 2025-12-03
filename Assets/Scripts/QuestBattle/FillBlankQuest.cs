using UnityEngine;

/// <summary>
/// 穴埋め問題用の判定クラス（拡張用）
/// </summary>
public class FillBlankQuest : MonoBehaviour
{
    public bool CheckAnswer(string input, string correct)
    {
        return input.Trim() == correct;
    }
}