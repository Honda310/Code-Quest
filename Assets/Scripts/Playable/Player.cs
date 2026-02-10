using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 【プレイヤー管理クラス】
/// キャラクターのステータス管理（HP, 攻撃力, 防御力）、
/// 入力に応じた移動処理、およびスプライトアニメーションの切り替えを担当します。
/// </summary>
public class Player : MonoBehaviour
{
    [NonSerialized] public string PlayerName;
    [NonSerialized] public int CurrentLv = 1;
    [NonSerialized] public int CurrentExp = 0;
    [NonSerialized] public int DebugLimit = 15;
    [NonSerialized] public Weapon CurrentEquipWeapon;
    [NonSerialized] public Accessory CurrentEquipAccessory;
    [NonSerialized] public string EquipWeaponName = "なし";
    [NonSerialized] public string EquipAccessoryName = "なし";
    public int NextExp
    {
        get { return CurrentLv*100; }
    }
    public int TotalExp
    {
        get { return ((CurrentLv-1)* (2 * 100 + (CurrentLv - 2) * 100) / 2)+CurrentExp; }
    }
    public int NextTotalExp
    {
        get { return CurrentLv * (2 * 100 + (CurrentLv - 1) * 100) / 2; }
    }
    public int MaxHP
    {
        get { return 95 + CurrentLv * 5; }
    } 
    [NonSerialized] public int CurrentHP = 100;
    public int BaseAtk
    {
        get { return 9 + CurrentLv; }
    } 
    public int BaseDef
    {
        get { return 9 + CurrentLv; }
    }

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
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb2d;

    // アニメーション制御用変数
    private int frame = 0;
    private string lastInputKey = "d";
    private int MovingIndex = 0;

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
        rb2d = GetComponent<Rigidbody2D>();
        sprites = new Dictionary<string, Sprite>()
        {
            { "w_0", Load("1forward_1stop") },          
            { "w_1", Load("1forward_2moveleftleg") },   
            { "w_2", Load("1forward_3moverightleg") },  

            { "s_0", Load("2back_1stop") },
            { "s_1", Load("2back_2moveleftleg") },
            { "s_2", Load("2back_3moverightleg") },

            { "a_0", Load("3left_1stop") },
            { "a_1", Load("3left_2move") },

            { "d_0", Load("4right_1stop") },
            { "d_1", Load("4right_2move") },
        };
        PlayerName = "コンカレ太郎";
    }

    /// <summary>
    /// Resourcesフォルダからスプライトをロード
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
        Vector2 direction = Vector2.zero; 
        frame++;
        if (frame == 15) MovingIndex = 1;      
        else if (frame == 30) MovingIndex = 0; 
        else if (frame == 45) MovingIndex = 2;
        else if (frame == 60) MovingIndex = 0; 

        if (frame >= 60) frame = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction.y = 1;
            lastInputKey = "w";
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            direction.y = -1;
            lastInputKey = "s";
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction.x = 1;
            lastInputKey = "d";
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction.x = -1;
            lastInputKey = "a";
        }
        int idx = (direction == Vector2.zero) ? 0 : MovingIndex;
        string key = lastInputKey;
        //※左右は左踏み出しと右踏み出しがなく、踏み出しが1つなので、丸めるための処理を行う部分
        if (key == "a" || key == "d")
        {
            idx = Mathf.Min(idx, 1);
        }

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
    /// <summary>
    /// 武器を装備し、攻撃力を更新する
    /// </summary>
    /// <param name="weapon">装備する武器データ</param>
    public void EquipWeapon(Item item)
    {
        Weapon weapon = item as Weapon;
        CurrentEquipWeapon = weapon;
        EquipWeaponName = weapon.ItemName;
        WeaponAtk = weapon.Atk;
        DebugLimit = Math.Max(15,weapon.TimeLimit);
    }

    /// <summary>
    /// 防具（アクセサリ）を装備し、防御力を更新する
    /// </summary>
    /// <param name="accessory">装備するアクセサリデータ</param>
    public void EquipAccessory(Item item)
    {
        Accessory accessory = item as Accessory;
        CurrentEquipAccessory = accessory;
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
            case 1:
                int heal = Mathf.Min(item.EffectSize, MaxHP - CurrentHP);
                CurrentHP += heal;
                break;

            case 2:
                ApplyTemporaryAtk(item.EffectSize);
                break;

            case 3:
                ApplyTemporaryDef(item.EffectSize);
                break;

            case 99:
                ClearBuffs();
                break;
        }
    }
    /// <summary>
    /// BattleManagerの戦闘勝利時に呼び出され、経験値を増加する。
    /// その後、増加した経験値をもとにレベルアップできなくなるまでレベルアップ処理を繰り返すメソッド。
    /// </summary>
    /// <param name="exp"></param>
    public void GainExperience(int exp)
    {
        CurrentExp += exp;
        while(NextExp <= CurrentExp)
        {
            CurrentExp -= NextExp;
            CurrentLv ++;
            CurrentHP = MaxHP;
        }
    }
    public void LoadFromSaveData(string playername,int currentlv,int currentexp,Weapon equipweapon,Accessory equipaccessory,int tempatk,int tempdef)
    {
        PlayerName = playername;
        CurrentLv = currentlv;
        CurrentExp = currentexp;
        CurrentEquipWeapon = equipweapon;
        CurrentEquipAccessory = equipaccessory;
        TemporaryAtk = tempatk;
        TemporaryDef = tempdef;
        DebugLimit = equipweapon.TimeLimit;
        EquipWeaponName = equipweapon.ItemName;
        WeaponAtk = equipweapon.Atk;
        EquipAccessoryName = equipaccessory.ItemName;
        AccessoryDef = equipaccessory.Def;
    }
}