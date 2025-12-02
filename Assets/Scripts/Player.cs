using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed = 1000f;
    public Rigidbody2D rb2d;
    public Sprite newSprite ;
    public int frame;
    public int legmove = 0;
    public int legmove_left = 0;
    public int legmove_right = 0;
    public string lastInputKey="right";

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 direction = Vector2.zero;
        frame++;
        if (frame == 15)
        {
            legmove = 1;
            legmove_left = 1;
        }
        else if (frame == 30)
        {
            legmove = 0;
            legmove_left = 0;
        }
        else if (frame == 45)
        {
            legmove = 1;
            legmove_right = 1;
        }
        else if (frame == 60)
        {
            legmove = 0;
            legmove_right = 0;
            frame = 0;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction.y = 1;
            forward();
            lastInputKey = "w";
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1;
            back();
            lastInputKey = "s";
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
            right();
            lastInputKey = "d";

        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
            left();
            lastInputKey = "a";
        }
        else
        {
            if (lastInputKey == "w")
            {
                newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_1forward_1stop");
                GetComponent<SpriteRenderer>().sprite = newSprite;
            }
            else if (lastInputKey == "s")
            {
                newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_2back_1stop");
                GetComponent<SpriteRenderer>().sprite = newSprite;
            }
            else if(lastInputKey == "d")
            {
                newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_4right_1stop");
                GetComponent<SpriteRenderer>().sprite = newSprite;
            }
            else if(lastInputKey == "a")
            {
                newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_3left_1stop");
                GetComponent<SpriteRenderer>().sprite = newSprite;
            }
        }
        direction = direction.normalized;
        rb2d.MovePosition(rb2d.position + direction * moveSpeed * Time.fixedDeltaTime);
    }
    void forward()
    {
        if (legmove_right==1)
        {
            newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_1forward_3moverightleg");
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else if (legmove_left == 1)
        {
            newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_1forward_2moveleftleg");
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else
        {
            newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_1forward_1stop");
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }
    void back()
    {
        if (legmove_right == 1)
        {
            newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_2back_3moverightleg");
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else if (legmove_left == 1)
        {
            newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_2back_2moveleftleg");
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else
        {
            newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_2back_1stop");
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }
    void left()
    {
        if (legmove == 1)
        {
            newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_3left_2move");
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else 
        {
            newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_3left_1stop");
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }
    void right()
    {
        if (legmove == 1)
        {
            newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_4right_2move");
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else
        {
            newSprite = Resources.Load<Sprite>("Image/Playable/1Player_1m_1normal_4right_1stop");
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }

    public Player(int maxhp, int currenthp, int currentatk, int weaponatk, int temporaryatk, int atk, int baseatk, int currentdef, int accessorydef, int temporarydef, int def, int basedef, int weaponequipmentid, int accessoryequipmentid, int timelimit)
    {
        MaxHP = maxhp;
        CurrentHP = currenthp;
        CurrentAtk = currentatk;
        WeaponAtk = weaponatk;
        TemporaryAtk = temporaryatk;
        Atk = atk;
        BaseAtk = baseatk;
        CurrentDef = currentdef;
        AccessoryDef = accessorydef;
        TemporaryDef = temporarydef;
        Def = def;
        BaseDef = basedef;
        WeaponEquipmentID = weaponequipmentid;
        AccessoryEquipmentID = accessoryequipmentid;
        TimeLimit = timelimit;
    }
    public int MaxHP { get; set; }
    public int CurrentHP { get; set; }
    public int CurrentAtk { get; set; }
    public int WeaponAtk { get; set; }
    public int TemporaryAtk { get; set; }
    public int Atk { get; set; }
    public int BaseAtk { get; set; }
    public int CurrentDef { get; set; }
    public int AccessoryDef { get; set; }
    public int TemporaryDef { get; set; }
    public int Def { get; set; }
    public int BaseDef { get; set; }
    public int WeaponEquipmentID { get; set; }
    public int AccessoryEquipmentID { get; set; }
    public int TimeLimit { get; set; }

    public void WeaponEquip(Weapon weapon, Player player)
    {
        player.WeaponEquipmentID = weapon.ItemID;
        player.WeaponAtk = weapon.Atk;
        player.Atk = player.WeaponAtk + player.BaseAtk;
        player.CurrentAtk = player.Atk + player.TemporaryAtk;
        player.TimeLimit = Math.Max(5, weapon.TimeLimit);
        Console.WriteLine($"åªç›äÓëbçUåÇóÕÅF{player.Atk}");
        Console.WriteLine($"åªç›çUåÇóÕÅF{player.CurrentAtk}");
        Console.WriteLine($"åªç›ÇÃâìöéÛïtéûä‘ÅF{player.TimeLimit}");
    }
    public void AccessoryEquip(Accessory accessory, Player player)
    {
        player.AccessoryEquipmentID = accessory.ItemID;
        player.AccessoryDef = accessory.Def;
        player.Def = player.AccessoryDef + player.BaseDef;
        player.CurrentDef = player.Def + player.TemporaryDef;
        Console.WriteLine($"åªç›äÓëbñhå‰óÕÅF{player.Def}");
        Console.WriteLine($"åªç›ñhå‰óÕÅF{player.CurrentDef}");
    }
}
