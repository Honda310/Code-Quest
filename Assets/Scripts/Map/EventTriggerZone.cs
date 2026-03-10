using UnityEngine;

/// <summary>
/// プレイヤーが侵入したときにイベントを発生させるトリガークラスです。
/// </summary>
public class EventTriggerZone : MonoBehaviour
{
    public int EventID;
    public bool OneTime = true;
    public Rigidbody2D rb;
    private bool triggered = false;
    public float dist = 1000.0f;
    private Transform target;
    private void Update()
    {
        Player player = Object.FindFirstObjectByType<Player>();
        if (player != null)
        {
            target = player.transform;
            Vector2 TargetPos = target.position;
            Vector2 pos = rb.position;
            Vector2 toPlayer = TargetPos - pos;
            dist = toPlayer.magnitude;
            if (dist < 160)
            {
                triggered = true;
            }
            else
            {
                triggered = false;
            }
        }
        if (triggered && OneTime)
        {
            GameManager.Instance.dialogueManager.StartEvent(EventID);
            OneTime = false;
        }
    }
}