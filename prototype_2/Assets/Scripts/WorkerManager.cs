using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public static List<GameObject> activeWorkers;
    public static int currentWorkerTier = 1;
    private static int workerCost = 100;

    private void Start() 
    {
        activeWorkers = new List<GameObject>();
    }

    /**
    *   Buy a worker if enough coins.
    */
    public static void BuyWorker(int qty)
    {
        int scaledWorkerCost = qty * workerCost * currentWorkerTier;
        if (AccountBalanceAI.money < scaledWorkerCost)
        {
            print("Not enough coins");
            return;
        }
        for (int i = 0; i < qty; i++)
        {
            GameObject worker = Instantiate(Resources.Load("Characters/Workers/Brigitte", typeof(GameObject))) as GameObject;
            worker.gameObject.GetComponent<Worker>().Name = Utility.GetRandomCharacterFirstName(Random.Range(0, Utility.characterNames.Length)); // Test, otherwise the player sets the name or it's random
            worker.gameObject.GetComponent<Worker>().RawBatchWorkPower = 100 * currentWorkerTier;
            AddWorker(worker);
        }
        AccountBalanceAI.UpdateMoney(-scaledWorkerCost);
        print($"Qty added: {qty}");
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
