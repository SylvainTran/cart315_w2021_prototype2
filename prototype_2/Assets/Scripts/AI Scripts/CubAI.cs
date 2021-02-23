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
        headingToTrainingCentreRestTarget = true;
        agent.SetDestination(trainingCentreRest.transform.position);
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
        if(headingToTrainingCentreRestTarget && agent.remainingDistance <= 0.03f) 
        {
            Wander();
        }
    }
}
