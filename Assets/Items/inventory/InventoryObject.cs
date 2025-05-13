using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "new_inventory", menuName = "inventory")]
public class InventoryObject : ScriptableObject {

    public List<InventorySlot> container = new List<InventorySlot>();
    public float coins;
    public void AddItem(ItemData _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i<container.Count; i++)
        {
            if(container[i].item == _item)
            {
                container[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }
        if(!hasItem)
        {
            container.Add(new InventorySlot(_item, _amount));
        }
    }
    public void AddCoins(float amount)
    {
        coins += amount;
    }
    
}

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;
    public InventorySlot(ItemData _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }

}