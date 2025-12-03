using UnityEngine;

/// <summary>
/// 宝箱の開閉状態と中身を管理するクラスです。
/// </summary>
public class TreasureBox : MonoBehaviour
{
    public string BoxID;
    public string ItemID;
    public bool IsOpened { get; private set; }

    public void Open()
    {
        if (!IsOpened)
        {
            IsOpened = true;
            Debug.Log($"宝箱 {BoxID} を開けました。中身: {ItemID}");
            // インベントリへの追加処理を呼び出す
        }
    }
}