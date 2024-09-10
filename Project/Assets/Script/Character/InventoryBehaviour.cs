using UnityEngine;

public abstract class InventoryBehaviour : MonoBehaviour
{
    public abstract int GetLastIndex();
    
    public abstract int GetNextIndex();
    
    public abstract WeaponBehaviour GetEquipped();

    public abstract int GetEquippedIndex();
    
    public abstract void Init(int equippedAtStart = 0);
    
    public abstract WeaponBehaviour Equip(int index);
}