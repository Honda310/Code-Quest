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

    Sprite Load(string name)
    {
        return Resources.Load<Sprite>($"Image/Playable/Neto/2Neto_1normal_{name}");
    }
    public Vector2 moveDir; // 入力や速度から更新すること

    void FixedUpdate()
    {
        if (moveDir.sqrMagnitude <= 0.0001f)
            return; // 移動していない時は処理しない

        // 角度取得&正規化
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        if ((angle >= 0f && angle < 45f) || (angle >= 315f && angle < 360f))
        {
            // Q1
            // 第1象限（0～45°, 315～360°）
        }
        else if (angle >= 45f && angle < 135f)
        {
            // Q2
            // 第2象限（45°～135°）
        }
        else if (angle >= 135f && angle < 225f)
        {
            // Q3
            // 第3象限（135°～225°）
        }
        else if (angle >= 225f && angle < 315f)
        {
            // Q4
            // 第4象限（225°～315°）
        }
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