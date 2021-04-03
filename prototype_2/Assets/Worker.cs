using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Worker : MonoBehaviour
{
    private NavMeshAgent agent;
    private string location;
    public string Location { get { return location; }}
    private string name;
    public string Name { get{ return name;} }
    private static int id = 0;
    public int Id { get{ return id;} }
    private int level = 0;
    public int Level { get{ return level;} }    
    private int experience = 0;
    public int Experience { get{ return experience;} }
    private int currentWorkBatch = 0;
    public int CurrentWorkBatch { get{ return currentWorkBatch; }}
    /// SCRIPTABLE OBJECT Locations
    public Workfield WORKFIELD_1;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveToDestination(Vector3 dest)
    {
        agent.SetDestination(dest);
    }

    public void InternalMoveToDestination(string location)
    {
        this.location = location;
    }

    public void CheckLocationAction()
    {
        switch(location)
        {
            case "WORKFIELD_1":
                StartCoroutine("StartWorking");            
            break;
            default:
            break;
        }
    }

    public IEnumerator StartWorking()
    {
        yield return new WaitForSeconds(3.0f);
        ++currentWorkBatch;
        print(name + $" work batch: : {currentWorkBatch} completed.");
        StartCoroutine("RestartWorking");
    }

    public IEnumerator RestartWorking()
    {
        yield return new WaitForSeconds(0.0f);
        StartCoroutine("StartWorking");
    }

    public string ToString()
    {
        return $"Name: {name} \nId: {id} \nLevel: {level} \nExperience: {experience} \nLocation: {location}";
    }

    public override bool Equals(object obj)
    {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        Worker otherWorker = (Worker) obj;
        if(otherWorker.Name != name || otherWorker.Id != id || otherWorker.Experience != experience || otherWorker.Level != level) {
            return false;
        }
        return true;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
