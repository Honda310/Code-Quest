using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public Weapon EquippedWeapon { get; private set; }
    public Accessory EquippedAccessory { get; private set; }

    public bool Equip(Item item)
    {
        switch (item)
        {
            case Weapon w:
                EquippedWeapon = w;
                return true;

            case Accessory a:
                EquippedAccessory = a;
                return true;
        }
        return false;
    }

    public void UnequipWeapon() => EquippedWeapon = null;
    public void UnequipAccessory() => EquippedAccessory = null;
}
