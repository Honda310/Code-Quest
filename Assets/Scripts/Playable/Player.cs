using UnityEngine;

/// <summary>
/// �y�v���C���[�z
/// ��l���L�����N�^�[�̃X�e�[�^�X�Ǘ��A�ړ��A�����������s���܂��B
/// </summary>
public class Player : MonoBehaviour
{
    [Header("��{�X�e�[�^�X")]
    public string PlayerName;
    public int MaxHP = 100;
    public int CurrentHP;
    public int BaseAtk = 10;
    public int BaseDef = 10;

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
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private CharacterAnimation charAnim;

    private void Start()
    {
        CurrentHP = MaxHP;
        rb = GetComponent<Rigidbody2D>();
        charAnim = GetComponent<CharacterAnimation>();

        // �ۑ����ꂽ���O��ǂݍ���
        PlayerName = PlayerPrefs.GetString("PlayerName", "Hero");
    }

    private void Update()
    {
        // ���͕������擾
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y).normalized;

        // �ړ�����
        Move(dir);

        // �A�j���[�V�����X�V
        if (charAnim != null)
        {
            charAnim.UpdateAnimation(dir);
        }
    }

    public void Move(Vector2 direction)
    {
        if (rb != null)
        {
            rb.linearVelocity = direction * moveSpeed;
        }
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