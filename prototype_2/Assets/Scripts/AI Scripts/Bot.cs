using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bot : MonoBehaviour
{
    protected NavMeshAgent agent;
    public GameObject target;
    public float wanderRadius;
    public float wanderDistance;
    public float wanderJitter;

    // Start is called before the first frame update
    public void Start()
    {
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    Vector3 wanderTarget = Vector3.zero;
    public void Wander()
    {
        if(!agent.isOnNavMesh) {
            return;
        }
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);
        Seek(targetWorld);
    }

    protected bool coolDown = false;
    public void BehaviourCoolDown()
    {
        coolDown = false;
    }

    public void CloseCorridors()
    {
        int closeDoorsAreaMask = (1 << 0) | (1 << 3);
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        nav.areaMask = closeDoorsAreaMask;
    }

    public void SetAllAreaMask()
    {
        int openAllAreasMask = (1 << 0) | (1 << 2) | (1 << 3);
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        nav.areaMask = openAllAreasMask;
    }
}
