using UnityEngine;

/// <summary>
/// プレイヤーが侵入したときにイベントを発生させるトリガークラスです。
/// </summary>
public class EventTriggerZone : MonoBehaviour
{
    public int EventID;
    public bool OneTime = true;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !triggered)
        {
            GameManager.Instance.eventManager.EventTrigger(EventID);

            if (OneTime)
            {
                triggered = true;
            }
        }
    }
}