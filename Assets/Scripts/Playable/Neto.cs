using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Neto : MonoBehaviour
{
    public int MaxHP;
    public int CurrentHP;
    public int BaseDef;
    private Accessory equipAccessory;

    public int AccessoryDef { get; private set; }
    public int TemporaryDef { get; private set; }
    public int CurrentDef => BaseDef + AccessoryDef + TemporaryDef;
    public Accessory CurrentEquipAccessory;
    public string EquipAccessoryName="なし";
    public Vector2 moveDelta;
    private Vector2 lastMoveDir = Vector2.right;

    private int frame = 0;
    private int MovingIndex = 0;
    public int quadrant=1;
    private static Neto instance;
    public Transform target;

    private Dictionary<string, Sprite> sprites;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーン切替後に Player を再取得
        Player player = Object.FindFirstObjectByType<Player>();
        if (player != null)
        {
            target = player.transform;
            Vector3 pos = target.position;
            transform.position = pos;
        }
    }
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
    private void Update()
    {
    }
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Dojo")
        {
            string key = $"4_0";
            if (sprites.ContainsKey(key)) GetComponent<SpriteRenderer>().sprite = sprites[key];
            return;
        }
        else
        {
            if (moveDelta.sqrMagnitude < 0.0001f)
            {
                MovingIndex = 0;

                float stopAngle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;
                if (stopAngle < 0) stopAngle += 360f;
                quadrant = GetQuadrant(stopAngle);

                string key = $"{quadrant}_0";
                if (sprites.ContainsKey(key)) GetComponent<SpriteRenderer>().sprite = sprites[key];

                return;
            }
        }
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
    public void EquipAccessory(Item item)
    {
        Accessory accessory = item as Accessory;
        CurrentEquipAccessory = accessory;
        EquipAccessoryName = accessory.ItemName;
        AccessoryDef = accessory.Def;
    }

    // FollowPlayer2D から呼ばれる
    public void UpdateMoveDelta(Vector2 delta)
    {
        moveDelta = delta;
    }

    public void ApplyTemporaryDef(int val)
    {
        TemporaryDef = val;
    }

    /// <summary>
    /// すべての一時的なバフ・デバフを解除する
    /// </summary>
    public void ApplyEffect(Item supportitem)
    {

        SupportItem item = supportitem as SupportItem;
        switch (item.EffectID)
        {
            case 1: // HP回復
                int heal = Mathf.Min(item.EffectSize, MaxHP - CurrentHP);
                CurrentHP += heal;
                break;

            case 2:
                break;

            case 3: // 防御力アップ
                ApplyTemporaryDef(item.EffectSize);
                break;

            case 99: // バフ解除（デバッグ完了など）
                ClearBuffs();
                break;
        }
    }
    public void ClearBuffs()
    {
        TemporaryDef = 0;
    }
    public void LoadFromSaveData(Accessory equipaccessory,int tempdef)
    {
        CurrentEquipAccessory = equipaccessory;
        TemporaryDef = tempdef;
        EquipAccessoryName = equipaccessory.ItemName;
        AccessoryDef = equipaccessory.Def;
    }
}
