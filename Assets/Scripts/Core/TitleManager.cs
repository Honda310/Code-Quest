using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 【タイトル画面】
/// ゲームの開始処理や、プレイヤー名の入力を担当します。
/// </summary>
public class TitleManager : MonoBehaviour
{
    public GameObject nameInputPanel; // 名前入力画面
    public InputField nameInputField; // 入力ボックス

    // 「はじめから」ボタン
    public void OnStartClicked()
    {
        nameInputPanel.SetActive(true);
    }

    // 「決定」ボタン
    public void OnNameDecided()
    {
        string name = nameInputField.text;

        // 名前が空でなければ保存してゲーム開始
        if (!string.IsNullOrEmpty(name))
        {
            PlayerPrefs.SetString("PlayerName", name);
            // 本編シーンへ移動（シーン名は適宜変更してください）
            SceneManager.LoadScene("GameScene");
        }
    }

    // 「終了」ボタン
    public void OnExitClicked()
    {
        Application.Quit();
    }
}