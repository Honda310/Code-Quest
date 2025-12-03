using UnityEngine;

/// <summary>
/// 【イベント管理】
/// ゲーム内のイベント（会話、ストーリー進行）をIDベースで管理します。
/// </summary>
public class EventManager : MonoBehaviour
{
    /// <summary>
    /// 指定されたIDのイベントを実行します
    /// </summary>
    public void EventTrigger(int eventId)
    {
        Debug.Log($"イベントID: {eventId} が発生しました。");

        // ここに if文 や switch文 でイベントごとの処理を書きます
        // 例: eventId == 1 ならオープニング会話を開始する、など
    }
}