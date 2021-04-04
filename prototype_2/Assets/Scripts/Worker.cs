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
    // This var is also set by the CLI, the player decides either a fixed amount of task progress required or a fixed amount of hours in-game to work
    private float currentTaskProgressHoursRequired = 8.0f; // this is an arg set by the player
    public float CurrentTaskProgressHoursRequired { get { return currentTaskProgressHoursRequired; } }
    private bool isWorking = false;
    public bool IsWorking = true;
    private float workBatchNextTickDelay = 3.0f;

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
                isWorking = true;
                StartCoroutine("StartWorking");            
            break;
            default:
            break;
        }
    }
    // Should this be called every next tick delay? Maybe a reason to do it is if the player somehow buffs the worker's work speed, one needs to recalculate
    // or if the player simply changes the number of progress hours required
    public float CalculateCurrentTaskProgressRequired()
    {   
        // 1 clock tick / 10 seconds
        // hence if nexttickdelay = 3s, then there are 3 1/3 work ticks / 1 clock tick or 10 seconds real time or 30 mins in-game
        // hence 6 2/3 work ticks / 1 hour in-game
        float workBatchesPerHour = SceneController.tickInterval / workBatchNextTickDelay * (60 / SceneController.minutesIncrementPerTick);
        print("Work batches per hour: " + workBatchesPerHour);
        float taskProgressPerHour = workBatchProcessingSpeed * rawBatchWorkPower * workBatchesPerHour;
        print("Task Progress per hour: " + taskProgressPerHour);
        print("Current Task Progress Required : " + taskProgressPerHour * currentTaskProgressHoursRequired);
        return taskProgressPerHour * currentTaskProgressHoursRequired;
    }

    public IEnumerator StartWorking()
    {
        if(!isWorking) {
            yield break;
        }
        yield return new WaitForSeconds(workBatchNextTickDelay);
        float progress = workBatchProcessingSpeed * rawBatchWorkPower; // 0.2 * 1 at level 0
        currentWorkBatchProgress += progress;
        ++currentWorkBatch;
        --stamina;
        print(name + $"Worker {id} work batch log:\n Actual Task Progress {currentWorkBatchProgress/CalculateCurrentTaskProgressRequired()*100}%, \nBatch completion: {currentWorkBatch/currentWorkBatchLimit*100}% completed.");
        // Check worker stamina before restarting
        if(HasStaminaLeft()) {
            // Stop condition 1: task progress required calculated is met
            if(currentWorkBatchProgress >= CalculateCurrentTaskProgressRequired()) {
                StopWorking();
                yield break;
            }
            // Stop condition 2: work batch limit is met
            if(currentWorkBatch < currentWorkBatchLimit) {
                StartCoroutine("RestartWorking");
            } else
            {
                print("Current work batch limit completed");
                StopWorking();
            }
        } else { // Stop condition 3: stamina out
            print("Out of stamina, MASTER");
            StopWorking();
        }
    }

    public void StopWorking()
    {
        if(isWorking) {
            isWorking = false;
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
