using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �y�v���C���[�z
/// ��l���L�����N�^�[�̃X�e�[�^�X�Ǘ��A�ړ��A�����������s���܂��B
/// </summary>
public class Player : MonoBehaviour
{
    [Header("��{�X�e�[�^�X")]
    public string PlayerName;
    public int MaxHP;
    public int CurrentHP;
    public int BaseAtk;
    public int BaseDef;

    // ������o�t�ő�������
    public int WeaponAtk { get; private set; }
    public int AccessoryDef { get; private set; }
    public int TemporaryAtk { get; set; }
    public int TemporaryDef { get; set; }

    // ���ۂɌv�Z�Ŏg�����݂̔\�͒l
    public int CurrentAtk
    {
        get { return BaseAtk + WeaponAtk + TemporaryAtk; }
    }
    public int CurrentDef
    {
        get { return BaseDef + AccessoryDef + TemporaryDef; }
    }

    [Header("�ړ��p�����[�^")]
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;

    public Rigidbody2D rb2d;
    public int frame;
    public string lastInputKey = "d";
    int MovingIndex = 0;
    Dictionary<string, Sprite> sprites;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        sprites = new Dictionary<string, Sprite>()
        {
            // forward
            { "w_0", Load("1forward_1stop") },
            { "w_1", Load("1forward_2moveleftleg") },
            { "w_2", Load("1forward_3moverightleg") },

            // back
            { "s_0", Load("2back_1stop") },
            { "s_1", Load("2back_2moveleftleg") },
            { "s_2", Load("2back_3moverightleg") },

            // left
            { "a_0", Load("3left_1stop") },
            { "a_1", Load("3left_2move") },

            // right
            { "d_0", Load("4right_1stop") },
            { "d_1", Load("4right_2move") },
        };
    }
    Sprite Load(string name)
    {
        return Resources.Load<Sprite>($"Image/Playable/1Player_1m_1normal_{name}");
    }
    void FixedUpdate()
    {
        Vector2 direction = Vector2.zero;
        frame++;

        if (frame == 15) MovingIndex = 1;
        else if (frame == 30) MovingIndex = 0;
        else if (frame == 45) MovingIndex = 2;
        else if (frame == 60) MovingIndex = 0;
        if (frame >= 60) frame = 0;
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
        int idx = (direction == Vector2.zero) ? 0 : MovingIndex;
        string key = lastInputKey;
        // 左右だけ2パターンしかないため補正
        if (key == "a" || key == "d") idx = Mathf.Min(idx, 1);

        GetComponent<SpriteRenderer>().sprite = sprites[$"{key}_{idx}"];
        Debug.Log(GetComponent<SpriteRenderer>().sprite);
        direction = direction.normalized;
        rb2d.MovePosition(rb2d.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    // ����𑕔�����
    public void EquipWeapon(Weapon weapon)
    {
        WeaponAtk = weapon.Atk;
        GameManager.Instance.uiManager.UpdateStatus(this, GameManager.Instance.neto);
    }

    // �h��i�A�N�Z�T���j�𑕔�����
    public void EquipAccessory(Accessory accessory)
    {
        AccessoryDef = accessory.Def;
        GameManager.Instance.uiManager.UpdateStatus(this, GameManager.Instance.neto);
    }

    // �ꎞ�I�ȃo�t��������
    public void ApplyTemporaryAtk(int val)
    {
        TemporaryAtk = val;
    }

    public void ApplyTemporaryDef(int val)
    {
        TemporaryDef = val;
    }

    // �o�t����������
    public void ClearBuffs()
    {
        TemporaryAtk = 0;
        TemporaryDef = 0;
    }
}