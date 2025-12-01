using UnityEngine;

public class Weapon : Item
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Weapon(int itemid, string itemname, int rarelity, int atk, int timelimit, string flavor) : base(itemid, itemname, rarelity, flavor)
    {
        Atk = atk;
        TimeLimit = timelimit;
    }

    public int Atk { get; }
    public int TimeLimit { get; }
}
