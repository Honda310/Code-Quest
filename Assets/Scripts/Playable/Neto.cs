using System.Collections.Generic;
using UnityEngine;

public class Neto : MonoBehaviour
{
    public int MaxHP;
    public int CurrentHP;
    public int BaseDef;

    public int AccessoryDef { get; private set; }
    public int TemporaryDef { get; private set; }
    public int CurrentDef => BaseDef + AccessoryDef + TemporaryDef;

    public Vector2 moveDelta;           // FollowPlayer から渡される実移動量
    private Vector2 lastMoveDir = Vector2.right;  // 停止時に向きを保持しておく

    private int frame = 0;
    private int MovingIndex = 0;
    public int quadrant=1;

    private Dictionary<string, Sprite> sprites;

    void Start()
    {
        sprites = new Dictionary<string, Sprite>()
        {
            { "2_0", Load("1forward_1stop") },
            { "2_1", Load("1forward_2moveleftleg") },
            { "2_2", Load("1forward_3moverightleg") },

            { "4_0", Load("2back_1stop") },
            { "4_1", Load("2back_2moveleftleg") },
            { "4_2", Load("2back_3moverightleg") },

            { "3_0", Load("3left_1stop") },
            { "3_1", Load("3left_2move") },

            { "1_0", Load("4right_1stop") },
            { "1_1", Load("4right_2move") },
        };
    }

    Sprite Load(string name)
    {
        return Resources.Load<Sprite>($"Image/Playable/Neto/2Neto_1normal_{name}");
    }

    void FixedUpdate()
    {
        // ■■■ 停止している場合（moveDelta = 0） ■■■
        if (moveDelta.sqrMagnitude < 0.0001f)
        {
            MovingIndex = 0;

            // ---- lastMoveDir を利用して向きを維持 ----
            float stopAngle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;
            if (stopAngle < 0) stopAngle += 360f;
            quadrant = GetQuadrant(stopAngle);

            string key = $"{quadrant}_0";
            if (sprites.ContainsKey(key))
                GetComponent<SpriteRenderer>().sprite = sprites[key];

            return;
        }

        // ■■■ 実際に動いている場合 ■■■

        // 進行方向を算出し lastMoveDir に保存
        Vector2 moveDir = moveDelta.normalized;
        lastMoveDir = moveDir;

        // アニメフレーム更新
        frame++;
        if (frame == 15) MovingIndex = 1;
        else if (frame == 30) MovingIndex = 0;
        else if (frame == 45) MovingIndex = 2;
        else if (frame == 60) { MovingIndex = 0; frame = 0; }

        // 向き
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        quadrant = GetQuadrant(angle);

        int idx = MovingIndex;

        // 左右（ quadrant 1 と 3 ）は 0/1 までしか存在しない
        if (quadrant == 1 || quadrant == 3)
            idx = Mathf.Min(idx, 1);

        // スプライト適用
        string spriteKey = $"{quadrant}_{idx}";
        if (sprites.ContainsKey(spriteKey))
            GetComponent<SpriteRenderer>().sprite = sprites[spriteKey];
    }


    // 角度 → 4象限
    int GetQuadrant(float angle)
    {
        if ((angle >= 0f && angle < 45f) || (angle >= 315f && angle < 360f)) return 1;
        if (angle >= 45f && angle < 135f) return 2;
        if (angle >= 135f && angle < 225f) return 3;
        return 4;
    }

    // FollowPlayer2D から呼ばれる
    public void UpdateMoveDelta(Vector2 delta)
    {
        moveDelta = delta;
    }

    // ヒント機能などがあればここに実装
    public void SpeakHint()
    {
        // 未実装
    }
    public void ApplyTemporaryDef(int val)
    {
        TemporaryDef = val;
    }

    /// <summary>
    /// すべての一時的なバフ・デバフを解除する
    /// </summary>
    public void ClearBuffs()
    {
        TemporaryDef = 0;
    }
}
