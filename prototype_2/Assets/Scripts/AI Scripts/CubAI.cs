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
        if (!agent.isOnNavMesh)
        {
            Invoke("DelayPlaceCharacterOnNavMesh", 3.6f);
        }
    }

    private void DelayPlaceCharacterOnNavMesh()
    {
        //Debug.Log("DelayPlaceCharacter on nav mesh");
        //GetComponent<Cub>().PlayDropFXThenDie();            
        agent.enabled = true;  
        agent.autoRepath = true;
        agent.autoBraking = true;
        agent.speed = 3.5f;    
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if(!hit.collider.gameObject.CompareTag("ground")) {
                return;
            }
            agent.Warp(hit.point);
            CancelInvoke();
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
}
