using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItemShop : MonoBehaviour
{
    // Events for the UI
    public delegate void UpdateUIBuyItemShop();
    public static event UpdateUIBuyItemShop UIOnBuyItemShop;

    void OnEnable()
    {
        ItemShopController.buyItemTemplate1 += GetItemTemplate1;
    }


    void OnDisable()
    {
        ItemShopController.buyItemTemplate1 -= GetItemTemplate1;
    }

    /**
    * Could be applied abstractedly to any game action
    */
    void GetItemTemplate1()
    {
        // Add to inventory if not full already
        if (ItemInventory.itemsInInventoryCount < ItemInventory.ITEM_MAX_AMOUNT)
        {
            Debug.Log("Adding an item to inventory");
            ++ItemInventory.itemsInInventoryCount;
            // This uses the Scriptable Object Constructor instead of invoking "new X()"
            ItemTemplate1 itemTemplate1 = ScriptableObject.CreateInstance<ItemTemplate1>();
            ItemInventory.AddToInventory(itemTemplate1);
            // Update UI via event
            UIOnBuyItemShop();
        }
        else
        {
            Debug.Log("Inventory full.");
        }
    }
}
