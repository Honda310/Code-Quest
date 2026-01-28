using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer2D : MonoBehaviour
{
    public Transform player;
    public float moveSpeed;
    public float stopDistance;
    public float smoothingFactor;
    public Vector2 smoothedDir = Vector2.zero;
    public Vector2 previousMoveDir;
    public LayerMask obstacleLayer;

    public Rigidbody2D rb;

    // ★ 前フレーム位置を保存する
    private Vector2 lastPosition;

    private Neto neto;  // キャラ本体

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastPosition = rb.position;
        neto = GetComponent<Neto>();
    }

    void FixedUpdate()
    {
        if (player == null) return;
        if (SceneManager.GetActiveScene().name == "Dojo")
        {

        }
        else
        {
            Vector2 pos = rb.position;
            Vector2 targetPos = player.position;
            Vector2 toPlayer = targetPos - pos;
            float dist = toPlayer.magnitude;
            if (dist <= stopDistance)
            {
                if (neto != null)
                    neto.UpdateMoveDelta(Vector2.zero);

                lastPosition = pos;
                return;
            }
            // 加減速
            if (dist >= 80)
            {
                moveSpeed = Mathf.Min(200.0f, moveSpeed + 10f);
            }
            else
            {
                moveSpeed = Mathf.Max(100.0f, moveSpeed - 10f);
            }
            Vector2 moveDir = toPlayer.normalized;
            smoothedDir = Vector2.Lerp(previousMoveDir, moveDir, smoothingFactor);
            previousMoveDir = smoothedDir;
            Vector2 newPos = pos + moveDir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
            Vector2 delta = newPos - lastPosition;
            if (neto != null) neto.UpdateMoveDelta(delta);
            lastPosition = newPos;
        }
    }
}


