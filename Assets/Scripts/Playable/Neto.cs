using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 【ネト】
/// パートナーキャラクターのクラスです。
/// </summary>
public class Neto : MonoBehaviour
{
    public int MaxHP;
    public int CurrentHP; 
    public int BaseDef;
    public int AccessoryDef { get; private set; }
    public int TemporaryDef { get; private set; }
    public int CurrentDef
    {
        get { return BaseDef + AccessoryDef + TemporaryDef; }
    }

    public Transform player;          // プレイヤー
    public float moveSpeed = 3f;      // 追従速度
    public float stopDistance = 1.5f;

    private Dictionary<string, Sprite> sprites;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

        // スプライト画像の読み込みと辞書への登録
        // Resourcesフォルダ内のパス: "Image/Playable/..."
        sprites = new Dictionary<string, Sprite>()
        {
            // --- 上向き (Wキー) ---
            { "w_0", Load("1forward_1stop") },          // 停止
            { "w_1", Load("1forward_2moveleftleg") },   // 左足踏み出し
            { "w_2", Load("1forward_3moverightleg") },  // 右足踏み出し

            // --- 下向き (Sキー) ---
            { "s_0", Load("2back_1stop") },
            { "s_1", Load("2back_2moveleftleg") },
            { "s_2", Load("2back_3moverightleg") },

            // --- 左向き (Aキー) ---
            { "a_0", Load("3left_1stop") },
            { "a_1", Load("3left_2move") },

            // --- 右向き (Dキー) ---
            { "d_0", Load("4right_1stop") },
            { "d_1", Load("4right_2move") },
        };
    }
    private void LateUpdate()
    {
        if (player == null) return;

        Vector2 pos = rb.position;
        Vector2 targetPos = player.position;

        // プレイヤーまでの方向と距離を取得
        Vector2 toPlayer = targetPos - pos;
        float dist = toPlayer.magnitude;

        // 停止距離以内なら動かない
        if (dist <= stopDistance)
            return;

        // 移動方向
        Vector2 moveDir = toPlayer.normalized;

        // 新しい位置
        Vector2 newPos = pos + moveDir * moveSpeed * Time.fixedDeltaTime;

        // MovePosition で移動（衝突に強い）
        rb.MovePosition(newPos);
    }
    Sprite Load(string name)
    {
        return Resources.Load<Sprite>($"Image/Playable/2Neto_1normal_{name}");
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