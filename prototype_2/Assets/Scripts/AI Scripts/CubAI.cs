using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CubAI : Bot
{
    public float maxFollowDistance = 2f;
    public GameObject trainingCentreRest;
    public bool headingToTrainingCentreRestTarget = false;
    private void Start() {
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
        if(!agent.isOnNavMesh || headingToTrainingCentreRestTarget && agent.isStopped) {
            headingToTrainingCentreRestTarget = false;
            agent.speed = 0.0f;
            // agent.Warp in case
            return;
        }
        agent.SetDestination(trainingCentreRest.transform.position);
        Debug.Log("Force moved");
    }
    private void Update()
    {
        if(!agent.isOnNavMesh) 
        {
            return;
        }
        if(!headingToTrainingCentreRestTarget) 
        {
            Wander();
        }
    }
}
