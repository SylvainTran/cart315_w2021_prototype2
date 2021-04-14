using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CubAI : Bot
{
    public float maxFollowDistance = 2f;
    public GameObject trainingCentreRest;
    public bool headingToTrainingCentreRestTarget = false;
    public bool selectedByPlayer = false; // If true, then use Setdestination command to move

    private new void Start() {
        trainingCentreRest = GameObject.FindGameObjectWithTag("trainingCentreRestTarget");    
        agent = this.GetComponent<NavMeshAgent>();      
    }
    /**
    * Bring the herd home!
    */
    public void MoveToTrainingCentreRest()
    {
        if(!agent.isOnNavMesh) {
            return;
        }
        headingToTrainingCentreRestTarget = true;
        InvokeRepeating("ForceMoveToTarget", 0.0f, 3.0f);
        wanderRadius = 0.3f;
        wanderDistance = 0.3f;
        wanderJitter = 0.3f;
        agent.speed = 3.5f;
        agent.autoBraking = true;
        agent.autoRepath = true;
    }

    public void ForceMoveToTarget()
    {
        if(!agent.isOnNavMesh)
        {
            return;
        }
        if(trainingCentreRest)
        {
            agent.SetDestination(trainingCentreRest.transform.position);
        }
        //Debug.Log("Force moved");
    }

    private void Update()
    {
        if (!headingToTrainingCentreRestTarget)
        {
            Wander();
        }
        if (agent.speed == 0 && headingToTrainingCentreRestTarget && agent.isOnNavMesh)
        {
            MoveToTrainingCentreRest();
        }
        if(GetComponent<Cub>().Satiety <= 50)
        {
            SeekFood();
        }
    }

    private void DelayPlaceCharacterOnNavMesh(GameObject liftedGameObject)
    {
        if(liftedGameObject.gameObject.name != this.gameObject.GetComponent<Cub>().characterName) return;
        Debug.Log("DelayPlaceCharacter on nav mesh");
        //GetComponent<Cub>().PlayDropFXThenDie();            
        agent.enabled = true;  
        agent.autoRepath = true;
        agent.autoBraking = true;
        agent.speed = 3.5f;    
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, Mathf.Infinity);
        foreach(RaycastHit hit in hits)
        {
            if(!hit.collider.gameObject.CompareTag("ground")) {
                return;
            }
            agent.Warp(hit.point);
        }
    }

    public void ForcePlaceCharacterOnNavMeshPoint()
    {
        agent.enabled = true;
        agent.autoRepath = true;
        agent.autoBraking = true;
        agent.speed = 3.5f;
        agent.Warp(GameObject.FindGameObjectWithTag("trainingCentreTarget").transform.position);
    }

    public void SeekFood()
    {
        float seekRange = 5.0f;
        //if hungry, scout for food within range if available

        GameObject[] foods = GameObject.FindGameObjectsWithTag("Fodder");
        for(int i = 0; i < foods.Length; i++)
        {
            if(Vector3.Distance(foods[i].gameObject.transform.position, transform.position) <= seekRange)
            {
                if(agent.isOnNavMesh)
                {
                    agent.SetDestination(foods[i].transform.position);
                }
                return;
            }
        }
    }
}
