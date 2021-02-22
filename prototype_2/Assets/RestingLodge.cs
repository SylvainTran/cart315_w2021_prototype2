using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Buildings just mutate cubs based on their location.
* They do not keep a copy of the cubsRooster, 
* because they don't need to, although a copy of the cubs
* that match this location can be used.
* Events can be attached to listen to cubs that move to this building.
*/
public class RestingLodge : Building
{
    public List<Character> charactersInThisBuilding; // all sorts of characters can end up in a building, causing havoc

    void OnEnable()
    {
        Cub.OnCharacterMoved += SetCharacterToThisLocation;
    }

    void OnDisable()
    {
        Cub.OnCharacterMoved -= SetCharacterToThisLocation;
    }

    private void Awake()
    {
        buildingName = "RESTING_LODGE";
        charactersInThisBuilding = new List<Character>();
    }

    public void SetCharacterToThisLocation(GameObject c)
    {
        Debug.Log($"Cub named {c.GetComponent<Character>().characterName} just moved to : {buildingName}");
        charactersInThisBuilding.Add(c.GetComponent<Character>());
    }

    private void OnMouseDown() 
    {
        Debug.Log($"{buildingName} was clicked by player.");
        // Display UI with cubs/other characters in the building
    }

    // Listener
    public void OnRestTick()
    {
        // Each tick of the clock, all the cubs currently in the building will be restored some amount of stamina etc.
    }
}
