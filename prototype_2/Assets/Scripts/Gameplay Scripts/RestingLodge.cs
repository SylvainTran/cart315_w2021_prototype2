using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/**
* Buildings just mutate cubs based on their location.
* They do not keep a copy of the cubsRooster, 
* because they don't need to, although a copy of the cubs
* that match this location can be used.
* Events can be attached to listen to cubs that move to this building.
*/
public class RestingLodge : Building
{
    public GameObject buildingMenu;
    public GameObject closeUpBuildingCam;
    protected GameObject[] labels;
  
    private void Awake()
    {
        buildingName = "RESTING_LODGE";
    }

    private void Start()
    {
        labels = GameObject.FindGameObjectsWithTag("buildingLabel");
    }

    private void OnMouseDown() 
    {
        Debug.Log($"{buildingName} was clicked by player.");
        // Prompt building menu or interactive cub placement?
        // Display UI with cubs/other characters in the building
        closeUpBuildingCam.GetComponent<CinemachineVirtualCamera>().Priority = 200;
    }

    public void ExitBuilding()
    {
        Debug.Log("Exiting Building");
        closeUpBuildingCam.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        foreach(GameObject go in labels)
        {
            go.SetActive(true);
        }
        buildingMenu.SetActive(false);     
    }

    // Listener
    public override void OnClockTick()
    {
        // Each tick of the clock, all the cubs currently in the building will be restored some amount of stamina etc.
    }
}
