using UnityEngine;

public class FollowPlayer2D : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float stopDistance = 1.5f;
    public float RayCastDistance;

    public LayerMask obstacleLayer;   // 壁レイヤーを指定
    public float avoidStrength = 0.7f; // 回避の強さ

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

        // プレイヤー方向
        Vector2 moveDir = toPlayer.normalized;

        // Raycast で障害物検出（前方）
        RaycastHit2D hit = Physics2D.Raycast(pos, moveDir, RayCastDistance, obstacleLayer);

        if (hit.collider != null)
        {
            // 壁にぶつかっているので、法線方向に押し出す
            Vector2 normal = hit.normal;
            moveDir = (moveDir + normal * avoidStrength).normalized;
        }

        // 位置更新
        Vector2 newPos = pos + moveDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}

