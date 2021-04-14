using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;

public class TrainingCentre : Building
{
    public GameObject closeUpBuildingCam;
    public GameObject buildingMenu;
    public GameObject restRooster;
    public GameObject trainRooster;    
    protected GameObject[] labels;
    public GameObject restSpawnPoint;
    public GameObject trainingCentreRestGate; // Carve at runtime
    public GameObject closeGateButton;    
    public GameObject openGateButton;
    public GameObject exitTrainingCentreButton;
    public GameObject globalCam;

    private void Awake()
    {
        buildingName = "TRAINING_CENTRE";
    }

    private void Start()
    {
        labels = GameObject.FindGameObjectsWithTag("buildingLabel");
        trainingCentreRestGate = GameObject.FindGameObjectWithTag("trainingCentreRestGate");        
        //buildingMenu = GameObject.Instantiate(Resources.Load("UI/TrainingCentreMenu")) as GameObject;
        interactibleState = Main.tutorialState == 2? true : false;
        if(Main.gameState == 1)
        {
            interactibleState = true;
        }
    }

    private void OnEnable() {
        SceneController.onClockTicked += OnClockTick;   
    }

    private void OnDisable() {
        SceneController.onClockTicked -= OnClockTick;   
    }

    private void OnMouseDown() 
    {
        Debug.Log($"{buildingName} was clicked by player.");
        if (!interactibleState) {
            return;
        }
        if (CustomEventController.eventLookedFor != null)
        {
            switch (CustomEventController.eventLookedFor)
            {
                case "ValidateOnMouseDownTarget":
                    if (ConversationController.dialogueActionExecutor.ActionTargetTag == this.gameObject.name)
                    {
                        print("We have a mousedown winner on the ProgramManagement building!");
                        CustomEventController.EnableConversationFlow();
                        CustomEventController.Flush();
                    }
                    break;
                default:
                    break;
            }
        }
        SetBuildingActive();
    }

    public void SwitchState()
    {           
        Main.playerState = 2;            
    }

    public void OnBuildingExit()
    {
        Debug.Log("Exiting Building");
        closeUpBuildingCam.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        GetComponent<BoxCollider>().enabled = true;
    
        foreach(Cub c in Main.currentCubRooster)
        {
            if(!c.isInTrainingProgram) {
                c.gameObject.GetComponent<CubAI>().headingToTrainingCentreRestTarget = false; 
                c.gameObject.GetComponent<NavMeshAgent>().speed = 3.5f;     
                c.gameObject.GetComponent<CubAI>().CancelInvoke(); // Cancel force moving to rest pen     
            } 
            else {
                // hold it there while it's training               
                c.gameObject.GetComponent<BoxCollider>().enabled = false;
                c.gameObject.GetComponent<CubAI>().enabled = false;
                c.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }           
        }
        Main.playerState = 0;
        CancelInvoke();
    }

    public void SetBuildingActive()
    {
        globalCam.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        closeUpBuildingCam.GetComponent<CinemachineVirtualCamera>().Priority = 400;
        Invoke("SwitchState", 3.0f);     
    }

    public void CloseRestGate()
    {
        // Add a carving navmesh obstacle to the gate
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().enabled = true;
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().carving = true;
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().carveOnlyStationary = true;
        //closeGateButton.SetActive(false);
        //openGateButton.SetActive(true);
        Quaternion target = Quaternion.Euler(0, -60.29f, 0);                   
        trainingCentreRestGate.transform.rotation = target;
    }

    public void OpenRestGate()
    {
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().carving = false;
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().carveOnlyStationary = false;
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().enabled = false;
        //closeGateButton.SetActive(true);
        //openGateButton.SetActive(false);
        Quaternion target = Quaternion.Euler(0, 57.15f, 0);            
        trainingCentreRestGate.transform.rotation = target;
    }
    
    public override void OnClockTick()
    {
        if(Main.currentCubRooster == null)
        {
            return;
        }
        if(Main.currentCubRooster.Count == 0)
        {
            return;
        }
        foreach(Cub c in Main.currentCubRooster)
        {
            if(!c.isInTrainingProgram) {
                continue;
            }
            c.leanness++;
            //c.cubProfileUI.GetComponent<UpdateCubProfileUI>().UpdateLeannessUI();            
            c.PlayFXThenDie("pickupStarFX");
            c.valueRating += c.leanness * 2;
        }
    }
}
