using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Clients : Building
{
    public GameObject closeUpBuildingCam;
    public GameObject buildingMenu;
    protected GameObject[] labels;
    public GameObject exitClientsButton;    

    private void Awake()
    {
        buildingName = "CLIENTS";
    }

    private void Start()
    {
        labels = GameObject.FindGameObjectsWithTag("buildingLabel");
    }

    private void OnMouseDown() 
    {
        Debug.Log($"{buildingName} was clicked by player.");
        closeUpBuildingCam.GetComponent<CinemachineVirtualCamera>().Priority = 200;
        foreach(GameObject go in labels)
        {
            go.SetActive(false);
        }
        buildingMenu.GetComponent<UpdateClientsUI>().ShowCanvas(true);
        exitClientsButton.SetActive(true);
        // Disable box collider temporarily to handle other colliders
        GetComponent<BoxCollider>().enabled = false;
        Invoke("SwitchState", 3.0f);
    }

    public void SwitchState()
    {           
        Main.playerState = 5; // CLIENTS           
    }

    public void OnBuildingExit()
    {
        Debug.Log("Exiting Building");
        closeUpBuildingCam.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        GetComponent<BoxCollider>().enabled = true;
        foreach(GameObject go in labels)
        {
            go.SetActive(true);
        }    
        buildingMenu.GetComponent<UpdateClientsUI>().ShowCanvas(false);
        exitClientsButton.SetActive(false);    
        Main.playerState = 0;
        CancelInvoke();
    }
}