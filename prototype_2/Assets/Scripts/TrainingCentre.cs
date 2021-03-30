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

    private void Awake()
    {
        buildingName = "TRAINING_CENTRE";
    }

    private void Start()
    {
        labels = GameObject.FindGameObjectsWithTag("buildingLabel");
        trainingCentreRestGate = GameObject.FindGameObjectWithTag("trainingCentreRestGate");        
        //buildingMenu = GameObject.Instantiate(Resources.Load("UI/TrainingCentreMenu")) as GameObject;
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
        // Prompt building menu or interactive cub placement?
        // Display UI with cubs/other characters in the building
        closeUpBuildingCam.GetComponent<CinemachineVirtualCamera>().Priority = 200;
        // Disable labels for clarity
        foreach(GameObject go in labels)
        {
            go.SetActive(false);
        }
        // Spawn Building Menu
        // buildingMenu.SetActive(true);
        // Show the cubs rooster hanging in the building REST pen.
        restRooster.SetActive(true);
        trainRooster.SetActive(true);    
        exitTrainingCentreButton.SetActive(true);
        // Disable box collider temporarily to handle other colliders
        GetComponent<BoxCollider>().enabled = false;
        OpenRestGate();
        foreach(Cub c in Main.currentCubRooster)
        {
            //c.gameObject.SetActive(true);
            if(c.gameObject.GetComponentInChildren<MeshRenderer>()) {
                c.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
            } else 
            {
                c.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            }
            c.gameObject.GetComponent<CubAI>().MoveToTrainingCentreRest();
            //c.transform.position = restSpawnPoint.transform.position;
        }
        Invoke("SwitchState", 3.0f);
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
        foreach(GameObject go in labels)
        {
            go.SetActive(true);
        }    
        OpenRestGate();
        closeGateButton.SetActive(false);
        restRooster.SetActive(false);  
        trainRooster.SetActive(false);     
        exitTrainingCentreButton.SetActive(false);    
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

    public void CloseRestGate()
    {
        // Add a carving navmesh obstacle to the gate
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().enabled = true;
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().carving = true;
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().carveOnlyStationary = true;
        closeGateButton.SetActive(false);
        openGateButton.SetActive(true);
        Quaternion target = Quaternion.Euler(0, -60.29f, 0);                   
        trainingCentreRestGate.transform.rotation = target;
    }

    public void OpenRestGate()
    {
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().carving = false;
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().carveOnlyStationary = false;
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().enabled = false;
        closeGateButton.SetActive(true);
        openGateButton.SetActive(false);
        Quaternion target = Quaternion.Euler(0, 57.15f, 0);            
        trainingCentreRestGate.transform.rotation = target;
    }
    
    public override void OnClockTick()
    {
        foreach(Cub c in Main.currentCubRooster)
        {
            if(!c.isInTrainingProgram) {
                continue;
            }
            //TODO c.currentTrainingProgram.Apply()
            c.performanceLevel++;
            c.cubProfileUI.GetComponent<UpdateCubProfileUI>().UpdatePerformanceLevelUI();            
            print("Cub IN training program: " + c);
            Debug.Log(c + " increased their performance level. Congratulations!");
            // play FX
            c.PlayFXThenDie("pickupStarFX");
            // increment their value rating
            c.valueRating += c.performanceLevel * 100;
        }
    }
}
