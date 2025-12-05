using UnityEngine;

public class FollowPlayer2D : MonoBehaviour
{
    public Transform player;
    public float moveSpeed;
    public float stopDistance;
    public float smoothingFactor;
    public Vector2 smoothedDir = Vector2.zero;
    public Vector2 previousMoveDir;
    public LayerMask obstacleLayer;   // 壁レイヤーを指定

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 pos = rb.position;
        Vector2 targetPos = player.position;

        // 距離
        Vector2 toPlayer = targetPos - pos;
        float dist = toPlayer.magnitude;

        // 停止距離以内なら動かない
        if (dist <= stopDistance)
            return;

        //一定距離以上離れた場合、加速する(近づくと元の速度に戻る)
        if(dist >= 80)
        {
            moveSpeed = Mathf.Min(200.0f,moveSpeed+10f);
        }
        else
        {
            moveSpeed = Mathf.Max(100.0f, moveSpeed - 10f);
        }

        Vector2 moveDir = toPlayer.normalized;
        smoothedDir = Vector2.Lerp(previousMoveDir, moveDir, smoothingFactor);
        previousMoveDir = smoothedDir;

        // 位置更新
        Vector2 newPos = pos + moveDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}

