using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public GameObject itemTemplate1;
    public GameObject placeItemAim;
    //private float delay = 45.0f;
    public enum ITEM_TYPES { StaminaPotion, CouragePotion };

    // Events for the UI
    public delegate void UpdateUIUsedItem();
    public static event UpdateUIUsedItem UIOnUsedItem;

    /**
     * ApplyUseItem()
     * 
     * Uses items on input
     */
    public void ApplyUseItem(int ITEM_TYPE)
    {
        if(ItemInventory.itemsInInventoryCount == 0)
        {
            Debug.Log("No items to use.");
            return;
        }
        // Check inventory for a trap of the type
        if(ITEM_TYPE == 0 && ItemInventory.HasItemsOfType<ItemTemplate1>())
        {
            Debug.Log("Using an item!");
            --ItemInventory.itemsInInventoryCount;
            int indexItem = ItemInventory.items.FindIndex(t => t.ItemID == 1);
            if (indexItem > -1)
            {
                Debug.Log("Removing item");
                ItemInventory.items.RemoveAt(indexItem);
                // Re-update UI
                UIOnUsedItem();
            }
        }
    }

    /**
     * FixedUpdate()
     * 
     * Native fixed update frame.
     * Variation ideas: Place at certain locations only constraint.
    */
    private void Update()
    {
        // Get input. TODO replace with new input action system.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ApplyUseItem((int)ITEM_TYPES.StaminaPotion);
        }
    }
}