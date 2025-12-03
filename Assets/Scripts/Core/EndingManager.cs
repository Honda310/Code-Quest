using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 【エンディング画面】
/// スタッフロールを流してタイトルに戻る演出を行います。
/// </summary>
public class EndingManager : MonoBehaviour
{
    public Text creditText; // スタッフロールのテキスト
    public float scrollSpeed = 20f; // 流れる速さ

    private void Start()
    {
        creditText.text = "CodeQuest\n\nTeam 7\n\nThank you for playing!";
        StartCoroutine(ScrollCredit());
    }

    // コルーチンを使ってテキストを動かす
    private IEnumerator ScrollCredit()
    {
        // Y座標がある程度行くまで上に移動し続ける
        while (creditText.transform.localPosition.y < 800)
        {
            creditText.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            yield return null; // 1フレーム待つ
        }

        // 少し待機
        yield return new WaitForSeconds(2.0f);

        // タイトルへ戻る
        SceneManager.LoadScene("TitleScene");
    }
}