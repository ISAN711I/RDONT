using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory")]
    public int coins = 0;
    public List<ItemType> items = new List<ItemType>();

    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Coins Collected: " + coins);
    }

    public void AddItem(ItemType newItem)
   {
     //   Item existingItem = items.Find(item => item.itemName == newItem.itemName);
      //  if (existingItem != null)
        ///{
          ////  existingItem.quantity += newItem.quantity;
       // }
       // else
        //{
         items.Add(newItem);
        //}
       // Debug.Log("Item Added: " + newItem.itemName + " x" + newItem.quantity);
  }
}
