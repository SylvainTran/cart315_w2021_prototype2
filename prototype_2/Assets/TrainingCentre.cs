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
    }

    public void OnMouseExit()
    {
        Debug.Log("Exiting Building");
        closeUpBuildingCam.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        foreach(GameObject go in labels)
        {
            go.SetActive(true);
        }
        //buildingMenu.SetActive(false);     
        restRooster.SetActive(false);  
        trainRooster.SetActive(false);         
        foreach(Cub c in Main.currentCubRooster)
        {
            // if(c.gameObject.GetComponentInChildren<MeshRenderer>()) {
            //     c.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
            // } else 
            // {
            //     c.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            // }
            c.gameObject.GetComponent<CubAI>().headingToTrainingCentreRestTarget = false;            
            //c.gameObject.SetActive(false);
            //c.transform.position = restSpawnPoint.transform.position;
        }
    }

    public void CloseRestGate()
    {
        // Add a carving navmesh obstacle to the gate
        trainingCentreRestGate.AddComponent<NavMeshObstacle>();
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().carving = true;
        trainingCentreRestGate.GetComponent<NavMeshObstacle>().carveOnlyStationary = true;
    }

    public override void OnClockTick()
    {
        

    }
}
