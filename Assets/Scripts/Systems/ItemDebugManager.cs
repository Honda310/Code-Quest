using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 【アイテムデバッグ機能】
/// アイテムを強化するためにコードを書くモードを管理します。
/// </summary>
public class ItemDebugManager : MonoBehaviour
{
    [SerializeField] private InputField codeInput;
    private Item targetItem;

    public void StartDebug(Item item)
    {
        targetItem = item;
        GameManager.Instance.SetMode(GameManager.GameMode.Debug);
        //GameManager.Instance.uiManager.ShowLog($"{item.ItemName} のデバッグを開始します。");
    }

    public void OnSubmit()
    {
        string code = codeInput.text;

        // 簡易判定: returnが含まれていれば成功とする（仮）
        if (code.Contains("return"))
        {
            //GameManager.Instance.uiManager.ShowLog("デバッグ成功！ アイテムが強化されました！");

            // TODO: アイテムIDを置換する処理などをここに記述

            GameManager.Instance.SetMode(GameManager.GameMode.Debug);
        }
        else
        {
            //GameManager.Instance.uiManager.ShowLog("コードに誤りがあります。");
        }
    }
}