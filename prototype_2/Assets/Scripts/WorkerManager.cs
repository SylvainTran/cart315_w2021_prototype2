using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public static List<GameObject> activeWorkers;

    private void Start() 
    {
        activeWorkers = new List<GameObject>();
        GameObject worker = Instantiate(Resources.Load("Characters/Workers/Brigitte", typeof(GameObject))) as GameObject;
        AddWorker(worker);               
    }

    public static void AddWorker(GameObject worker)
    {
        activeWorkers.Add(worker);
        print(worker.GetComponent<Worker>().ToString());
    }

    public static void RemoveWorker(GameObject   worker)
    {
        activeWorkers.Remove(worker);
    }
}
