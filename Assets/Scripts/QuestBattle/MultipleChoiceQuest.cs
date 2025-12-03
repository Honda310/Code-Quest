using UnityEngine;

/// <summary>
/// 4択問題用の判定クラス（拡張用）
/// </summary>
public class MultipleChoiceQuest : MonoBehaviour
{
    public bool CheckAnswer(int select, int correct)
    {
        return select == correct;
    }
}