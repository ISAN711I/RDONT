using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Needed for pointer events
using TMPro; // If you're using TextMeshPro
using System.Collections.Generic;

public class Shop_Item_behaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryObject player_inventory;
    public ItemData my_item;
    public Splitter_list msl;
    public List<ItemData> basic = new List<ItemData>();
    public List<ItemData> uncommon = new List<ItemData>();
    public List<ItemData> rare = new List<ItemData>();
    public List<ItemData> legendary = new List<ItemData>();


    public float price;

    public TMP_Text descriptionText; // Assign this in the inspector
    public TMP_Text nameText;
    void Start()
    {
        float rng = Random.value;

        if (rng < 0.7f && basic.Count > 0)
        {
            price = 30;
            my_item = basic[Random.Range(0, basic.Count)];
        }
        else if (rng < 0.85f && uncommon.Count > 0)
        {
            price = 50;
            my_item = uncommon[Random.Range(0, uncommon.Count)];
        }
        else if (rng < 0.95f && rare.Count > 0)
        {
            price = 70;
            my_item = rare[Random.Range(0, rare.Count)];
        }
        else if (legendary.Count > 0)
        {
            price = 100;
            my_item = legendary[Random.Range(0, legendary.Count)];
        }
        else
        {
            Debug.LogWarning("No valid items available in selected rarity list.");
        }

        GetComponent<Image>().sprite = my_item.Display;
        nameText.text = my_item.Name + price;
    }

    public void buyItem()
    {
        if (player_inventory.coins >= price)
        {
            player_inventory.AddCoins(-price);

            if(my_item.mysplitter.prefab != null)
            {
                msl.AddSplitter(my_item.mysplitter);
            }

            player_inventory.AddItem(my_item, 1);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Can't afford");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionText.text = my_item.description;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionText != null)
        {
            descriptionText.text = "";
        }
    }
}
