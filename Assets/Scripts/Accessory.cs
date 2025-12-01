using UnityEngine;

public class Accessory : Item
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public Accessory(int itemid, string itemname, int rarelity, int def, string flavor) : base(itemid, itemname, rarelity, flavor)
    {
        Def = def;
    }

    public int Def { get; }
}
