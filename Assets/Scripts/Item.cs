using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Item(int itemid, string itemname, int rarelity, string flavor)
    {

        ItemID = itemid;
        ItemName = itemname;
        Rarelity = rarelity;
        Flavor = flavor;
    }
    public int ItemID { get; }
    public string ItemName { get; }
    public int Rarelity { get; }
    public string Flavor { get; }
}
