using UnityEngine;

/// <summary>
/// 【道場機能】
/// ネト先生による学習モードを管理します。
/// </summary>
public class DojoManager : MonoBehaviour
{
    public void OpenDojo()
    {
        GameManager.Instance.uiManager.ToggleDojo(true);
    }

    public void SelectTopic(string topic)
    {
        GameManager.Instance.uiManager.ShowLog($"{topic} の学習を開始します。");
        // 解説画面などを表示する処理へ
    }
}