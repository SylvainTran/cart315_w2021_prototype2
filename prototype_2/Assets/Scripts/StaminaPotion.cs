using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* This class inherits from Item as a
* serialized scriptable object. Values are initialized
* from the inspector for ease.
*/
public class StaminaPotion : Item
{
    /**
    * Default constructor. 
    * Used to get an instance of this trap 
    * in the player inventory mainly.
    */
    public StaminaPotion() : base()
    {

    }
}
