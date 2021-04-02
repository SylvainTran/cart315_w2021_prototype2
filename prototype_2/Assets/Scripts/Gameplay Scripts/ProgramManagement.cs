using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ProgramManagement : Building
{
    public GameObject closeUpBuildingCam;
    public GameObject buildingMenu;
    public GameObject inactiveTrainingPrograms;
    public GameObject activeTrainingPrograms;    
    protected GameObject[] labels;
    public GameObject exitProgramManagementButton;    

    private void Awake()
    {
        buildingName = "PROGRAM_MANAGEMENT";
    }

    private void Start()
    {
        labels = GameObject.FindGameObjectsWithTag("buildingLabel");
    }

    private void OnMouseDown() 
    {
        if(!interactibleState)
        {
            return;
        }
        Debug.Log($"{buildingName} was clicked by player.");
        closeUpBuildingCam.GetComponent<CinemachineVirtualCamera>().Priority = 200;
        //buildingMenu.GetComponent<UpdateProgramManagementUI>().ShowCanvas(true);        
        exitProgramManagementButton.SetActive(true);
        // Disable box collider temporarily to handle other colliders
        Invoke("SwitchState", 3.0f);
        ConversationController.pauseConversations = false;
    }

    public void SwitchState()
    {           
        Main.gameState = 3; // PROGRAM_MANAGEMENT           
    }

    public void OnBuildingExit()
    {
        Debug.Log("Exiting Building");
        closeUpBuildingCam.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        buildingMenu.GetComponent<UpdateProgramManagementUI>().ShowCanvas(false);   
        exitProgramManagementButton.SetActive(false);    
        Main.gameState = 1;
        CancelInvoke();
    }
}
