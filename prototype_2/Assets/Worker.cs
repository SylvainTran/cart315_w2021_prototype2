using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Worker : MonoBehaviour
{
    private NavMeshAgent agent;
    private string location;
    public string Location { get { return location; } }
    private string name;
    public string Name { get { return name;} }
    private static int id = 0;
    public int Id { get { return id;} }
    private int level = 0;
    public int Level { get { return level;} }    
    private int experience = 0;
    public int Experience { get { return experience;} }
    private float currentWorkBatch = 0;
    public float CurrentWorkBatch { get { return currentWorkBatch; } }
    private float currentWorkBatchLimit = 10.0f;
    public float CurrentWorkBatchLimit { get { return currentWorkBatchLimit; } set { if(value >= 0) currentWorkBatchLimit = value; } }
    private float workBatchProcessingSpeed = 0.2f; // Very slow at work initially
    public float WorkBatchProcessingSpeed = 0.2f;
    private int rawBatchWorkPower = 1; // base tick per batch
    public int RawBatchWorkPower { get {return rawBatchWorkPower;} }    
    private int stamina = 15; // Very weak workers initially, player needs to progress/upgrade!
    public int Stamina { get { return stamina;} }
    private float currentWorkBatchProgress = 0f;
    public float CurrentWorkBatchProgress { get { return currentWorkBatchProgress; } }
    private float currentTaskProgressRequired = 100f;
    public float CurrentTaskProgressRequired { get { return currentTaskProgressRequired; } }
    
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
        float progress = workBatchProcessingSpeed * rawBatchWorkPower; // 0.2 * 1 at level 0
        currentWorkBatchProgress += progress;
        ++currentWorkBatch;
        --stamina;
        print(name + $"Worker {id} work batch log:\n Actual Task Progress {currentWorkBatchProgress/currentTaskProgressRequired*100}%, \nBatch completion: {currentWorkBatch/currentWorkBatchLimit*100}% completed.");
        // Check worker stamina before restarting
        if(HasStaminaLeft()) {
            if(currentWorkBatch < currentWorkBatchLimit) {
                StartCoroutine("RestartWorking");
            } else
            {
                print("Current work batch limit completed");
            }
        } else {
            print("Out of stamina, MASTER");
        }
    }

    public bool HasStaminaLeft()
    {
        return stamina > 0;
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
