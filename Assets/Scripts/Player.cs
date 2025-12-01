using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Console.WriteLine($"Œ»İŠî‘bUŒ‚—ÍF{player.Atk}");
        Console.WriteLine($"Œ»İUŒ‚—ÍF{player.CurrentAtk}");
        Console.WriteLine($"Œ»İ‚Ì‰ğ“šó•tŠÔF{player.TimeLimit}");
    }
    public void AccessoryEquip(Accessory accessory, Player player)
    {
        player.AccessoryEquipmentID = accessory.ItemID;
        player.AccessoryDef = accessory.Def;
        player.Def = player.AccessoryDef + player.BaseDef;
        player.CurrentDef = player.Def + player.TemporaryDef;
        Console.WriteLine($"Œ»İŠî‘b–hŒä—ÍF{player.Def}");
        Console.WriteLine($"Œ»İ–hŒä—ÍF{player.CurrentDef}");
    }
}
