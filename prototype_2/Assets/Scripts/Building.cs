using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Building : MonoBehaviour
{
    protected string buildingName;
    public List<Character> charactersInThisBuilding; // all sorts of characters can end up in a building, causing havoc

    private void Start()
    {
        charactersInThisBuilding = new List<Character>();
    }

    void OnEnable()
    {
        Cub.OnCharacterMoved += SetCharacterToThisLocation;
    }

    void OnDisable()
    {
        Cub.OnCharacterMoved -= SetCharacterToThisLocation;
    }

    public void SetCharacterToThisLocation(GameObject c)
    {
        Debug.Log($"Cub named {c.GetComponent<Character>().characterName} just moved to : {buildingName}");
        charactersInThisBuilding.Add(c.GetComponent<Character>());
    }

    // Listener
    public virtual void OnClockTick()
    {
        // Each tick of the clock, all the cubs currently in the building will be restored some amount of stamina etc.
    }
}
