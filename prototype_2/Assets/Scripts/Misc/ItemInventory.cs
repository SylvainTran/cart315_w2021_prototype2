using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public static int ITEM_MAX_AMOUNT = 30; // Max items
    public static int itemsInInventoryCount = 0; // How many items currently have
    public static List<Item> items;

    private void Start()
    {
        items = new List<Item>();
    }

    // LINQ command to check if any item of type T is in the inventory
    public static bool HasItemsOfType<T>() where T : Item
    {
        return items.Any(t => t is T);
    }

    public static void AddToInventory(Item item)
    {
        items.Add(item);
    }
}
