using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // InputSystemを使用する場合は必要

/// <summary>
/// 【プレイヤー管理クラス】
/// キャラクターのステータス管理（HP, 攻撃力, 防御力）、
/// 入力に応じた移動処理、およびスプライトアニメーションの切り替えを担当します。
/// </summary>
public class Player : MonoBehaviour
{
    // --- ステータス関連 ---
    public string PlayerName; // プレイヤー名
    public int MaxHP=100;         // 最大HP
    public int CurrentHP=100;     // 現在HP
    public int BaseAtk=10;       // 基礎攻撃力
    public int BaseDef=10;       // 基礎防御力
    public int DebugLimit = 5;
    [NonSerialized] public string EquipWeaponName="なし";
    [NonSerialized] public string EquipAccessoryName="なし";

    public int WeaponAtk { get; private set; }
    public int AccessoryDef { get; private set; }

    public int TemporaryAtk { get; set; }
    public int TemporaryDef { get; set; }

    /// <summary>
    /// 現在の最終攻撃力（基礎 + 武器 + バフ）
    /// </summary>
    public int CurrentAtk
    {
        get { return BaseAtk + WeaponAtk + TemporaryAtk; }
    }

    /// <summary>
    /// 現在の最終防御力（基礎 + 防具 + バフ）
    /// </summary>
    public int CurrentDef
    {
        get { return BaseDef + AccessoryDef + TemporaryDef; }
    }

    // --- 移動・アニメーション関連 ---
    [Header("移動パラメータ")]
    [SerializeField] private float moveSpeed; // 移動速度
    private Rigidbody2D rb2d; // 物理演算用コンポーネント

    // アニメーション制御用変数
    private int frame = 0; // アニメーションフレームカウンタ
    private string lastInputKey = "d"; // 最後に押された方向キー（待機時の向き用）
    private int MovingIndex = 0; // 歩行アニメーションの段階 (0:停止, 1:右足, 2:左足など)

    // スプライト画像をキャッシュする辞書
    // Key: "方向キー_インデックス" (例: "w_1"), Value: 対応するSprite
    private Dictionary<string, Sprite> sprites;

    private static Player instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // コンポーネントの取得
        rb2d = GetComponent<Rigidbody2D>();

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

    /// <summary>
    /// Resourcesフォルダからスプライトをロードするヘルパーメソッド
    /// </summary>
    Sprite Load(string name)
    {
        return Resources.Load<Sprite>($"Image/Playable/Player/1Player_1m_1normal_{name}");
    }
    private void Update()
    {
        
    }
    /// <summary>
    /// 物理演算フレームごとの更新処理 (移動・アニメーション)
    /// </summary>
    void FixedUpdate()
    {
        if (!(GameManager.Instance.CurrentMode == GameManager.GameMode.Field)) return; 
        Vector2 direction = Vector2.zero; // 移動方向ベクトル

        // --- アニメーションのコマ送り処理 ---
        frame++;

        // 15フレームごとにアニメーションパターンを切り替える
        if (frame == 15) MovingIndex = 1;      // 足踏み1
        else if (frame == 30) MovingIndex = 0; // 停止位置に戻る
        else if (frame == 45) MovingIndex = 2; // 足踏み2 (逆足)
        else if (frame == 60) MovingIndex = 0; // 停止位置に戻る

        // 60フレームで1ループ
        if (frame >= 60) frame = 0;

        // --- キー入力の判定 ---
        // 上下左右の入力を検知し、移動方向と「最後の入力キー」を更新
        if (Input.GetKey(KeyCode.W))
        {
            direction.y = 1;
            lastInputKey = "w";
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1;
            lastInputKey = "s";
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
            lastInputKey = "d";
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
            lastInputKey = "a";
        }

        // --- スプライトの切り替え ---
        // 移動していない場合は停止スプライト(Index 0)、移動中はコマ送り(MovingIndex)を使用
        int idx = (direction == Vector2.zero) ? 0 : MovingIndex;
        string key = lastInputKey;

        // 左右移動のアニメーションは2パターンしかない（0と1）ため、インデックスを制限する
        if (key == "a" || key == "d")
        {
            idx = Mathf.Min(idx, 1);
        }

        // 辞書から該当するスプライトを取得して反映
        string spriteKey = $"{key}_{idx}";
        if (sprites.ContainsKey(spriteKey))
        {
            GetComponent<SpriteRenderer>().sprite = sprites[spriteKey];
        }

        direction = direction.normalized;

        Vector2 newPos = rb2d.position + direction * moveSpeed * Time.fixedDeltaTime;

        newPos.x = Mathf.Round(newPos.x);
        newPos.y = Mathf.Round(newPos.y);

        rb2d.MovePosition(newPos);
    }

    // ==========================================
    // ステータス操作メソッド群
    // ==========================================

    /// <summary>
    /// 武器を装備し、攻撃力を更新する
    /// </summary>
    /// <param name="weapon">装備する武器データ</param>
    public void EquipWeapon(Item item)
    {
        Weapon weapon = item as Weapon;
        EquipWeaponName = weapon.ItemName;
        WeaponAtk = weapon.Atk;
        DebugLimit = weapon.TimeLimit;
    }

    /// <summary>
    /// 防具（アクセサリ）を装備し、防御力を更新する
    /// </summary>
    /// <param name="accessory">装備するアクセサリデータ</param>
    public void EquipAccessory(Item item)
    {
        Accessory accessory = item as Accessory;
        EquipAccessoryName = accessory.ItemName;
        AccessoryDef = accessory.Def;
    }

    /// <summary>
    /// 一時的な攻撃力バフを適用する
    /// </summary>
    public void ApplyTemporaryAtk(int val)
    {
        TemporaryAtk = val;
    }

    /// <summary>
    /// 一時的な防御力バフを適用する
    /// </summary>
    public void ApplyTemporaryDef(int val)
    {
        TemporaryDef = val;
    }

    /// <summary>
    /// すべての一時的なバフ・デバフを解除する
    /// </summary>
    public void ClearBuffs()
    {
        TemporaryAtk = 0;
        TemporaryDef = 0;
    }
    public void ApplyEffect(Item supportitem)
    {

        SupportItem item = supportitem as SupportItem;
        switch (item.EffectID)  
        {
            case 1: // HP回復
                int heal = Mathf.Min(item.EffectSize, MaxHP - CurrentHP);
                CurrentHP += heal;
                break;

            case 2: // 攻撃力アップ
                ApplyTemporaryAtk(item.EffectSize);
                break;

            case 3: // 防御力アップ
                ApplyTemporaryDef(item.EffectSize);
                break;

            case 99: // バフ解除（デバッグ完了など）
                ClearBuffs();
                break;
        }
    }
}