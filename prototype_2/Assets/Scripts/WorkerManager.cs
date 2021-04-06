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
        worker.gameObject.GetComponent<Worker>().Name = "Brigitte"; // Test, otherwise the player sets the name or it's random
        worker.gameObject.GetComponent<Worker>().RawBatchWorkPower = 100;
        AddWorker(worker);               
    }

    public static void AddWorker(GameObject worker)
    {
        activeWorkers.Add(worker);
        print(worker.GetComponent<Worker>().ToString());
        AccountBalanceAI.UpdateWorkerCount(1);
    }

    public static void RemoveWorker(GameObject   worker)
    {
        activeWorkers.Remove(worker);
        AccountBalanceAI.UpdateWorkerCount(-1);
    }
}
