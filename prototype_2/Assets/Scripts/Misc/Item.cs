using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* The definition of traps down the inheritance chain do not contain
* behaviours, only data specifics.
*/
[CreateAssetMenu(fileName = "Item", menuName = "Gameplay/Item", order = 1)]
public class Item : ScriptableObject  
{
    /**
    * Used for equality checks by consumers like
    * UseItem(). We need to know if a certain trap type exists
    * in the inventory. The trap ID is defined for each trap type to
    * allow checking its type.
    */
    private int itemID = 1;
    public int ItemID { get { return itemID; } }

    /**
    * Instantiation related.
    */
    [Header("Prefabs for Instantiation and FX")]
    public GameObject effectPrefab1;
    public AudioClip soundFX1;
    public GameObject textPrefab1;
    public string _textPrefab1;

    /**
    * Default constructor. Initialization is done via
    * the inspector since this is a scriptable object,
    * but can be used for private details later.
    */
    public Item() 
    {

    }

    public override bool Equals(System.Object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Item i = (Item)obj;
            return (itemID == i.itemID);
        }
    }
}
