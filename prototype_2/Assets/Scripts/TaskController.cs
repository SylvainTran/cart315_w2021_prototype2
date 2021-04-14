using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    TASKS inherit from goals.
    They're not missions, they're just things that
    reward the player / workers in a non-special way.
*/
public class TaskController : MonoBehaviour
{
    // Dispatch custom tasks with themes
    // Receive tasks completed and rewards player
    public static Queue<Task> tasksQueue = new Queue<Task>();
    public delegate void OnTaskStatisticsUpdated();
    public static event OnTaskStatisticsUpdated onTaskStatisticsUpdated;

    private void OnEnable() {
        Worker.onTaskFinished += TaskFinished;
        Worker.onBatchFinished += BatchFinished;
    }

    private void OnDisable() {
        Worker.onTaskFinished += TaskFinished;
        Worker.onBatchFinished += BatchFinished;
    }

    public static Task GetTaskFromQueue()
    {
        return tasksQueue.Dequeue();
    }

    // Management -> Create task -> Dispatch/Work
    public static void InitTask(Task task, List<string> args)
    {
        // createTask(int ProgressHoursRequired, int WorkBatchLimit);
        if(args[0].Equals(string.Empty)) 
        {   
            // "createTask(, args2)" case
            // Default work target, but we define work hours for args 2
            task.CurrentWorkBatchLimit = Single.Parse(args[1]);
        } else if(args[1].Equals(string.Empty))
        {
            // "name.work(args1,)" case
            // Defined work target, but default work hours for args 2
            task.ProgressHoursRequired = Single.Parse(args[0]);
        } else if(!args[0].Equals(string.Empty) && !args[1].Equals(string.Empty)) // "createWork(args1,args2)" case
        {
            task.ProgressHoursRequired = Single.Parse(args[0]);
            task.CurrentWorkBatchLimit = Single.Parse(args[1]);
        }
        print(task.ToString());
        tasksQueue.Enqueue(task);
        AccountBalanceAI.UpdateTaskCount(1);
    }

    public void BatchFinished(Worker worker, Task task)
    {
        // Could end before task is completely finished, but here the AI goes to check out the gathered materials to the storehouse
        // Chance reward
        Reward(worker, task, 1f);
        StopCoroutine("StartNewWorkBatch");
        StartCoroutine(StartNewWorkBatch(worker, task));
        // Refresh UI
        onTaskStatisticsUpdated();
    }

    public IEnumerator StartNewWorkBatch(Worker worker, Task task)
    {
        yield return new WaitForSeconds(task.WorkBatchCooldown);
        print($"Worker {worker.characterName} has a new work batch to work on!");
        worker.CurrentTask.CurrentWorkBatch = 0f;
        // Check between batches if still has enough money to complete task, otherwise pay what's been done so far
        if(AccountBalanceAI.money < worker.CalculateCoinRequired(task))
        {
            print($"Insufficient funds to pay the rest of the task. Pausing task until can pay off remaining hours required.");
            float moneyEarned = worker.CalculateCoinRequired(task) - (10.0f * WorkerManager.currentWorkerTier * task.CurrentWorkBatchProgress / worker.currentTaskProgressPerHour);
            // if cannot pay moneyEarned so far, frustrate the worker and risk losing them
            if(AccountBalanceAI.money < moneyEarned)
            {
                print($"Worker {worker.characterName} hates you");
                // TODO Decrease relationship points
            } else
            {
                AccountBalanceAI.UpdateMoney(-moneyEarned);
                worker.CoinInInventory += moneyEarned;
            }
            worker.StopWorking();
        }
        worker.StartWorkCoroutine = worker.StartCoroutine("StartWorking");
        worker.IsWorking = true;
        print($"{worker.characterName} current working task progress hours required: {worker.CurrentTask.ProgressHoursRequired}");
    }

    public void TaskFinished(Worker worker, Task task)
    {
        // Reward worker and player both!
        PayWorkerSalary(worker, task);
        Reward(worker, task, 1f);
        IncreaseWorkerExperience(worker, task);
        // Do other things too
        AccountBalanceAI.UpdateTaskCount(-1);

        // Random chance to log various complaints or comments to the player
        // about their task work
        onTaskStatisticsUpdated();
    }

    public void PayWorkerSalary(Worker worker, Task task)
    {
        float salaryEarned = 10.0f * WorkerManager.currentWorkerTier * task.ProgressHoursRequired;
        AccountBalanceAI.UpdateMoney(-salaryEarned);
        worker.CoinInInventory += salaryEarned;
    }

    public void IncreaseWorkerExperience(Worker worker, Task task)
    {
        float experienceEarned = 10 ^ (worker.Level * task.TaskRewardTier); //exponential base 10
        experienceEarned *= task.ProgressHoursRequired;
        worker.Experience += experienceEarned;
        print($"Worker gained: {experienceEarned}!");
    }

    public GameObject fodderPrefab;
    /**
     * Chance reward check with the probability given,
     * to the worker and the task assigned.
     * @param worker : Worker
     * @param task : Task
     * @param probability : Float
     */
    public void Reward(Worker worker, Task task, float probability)
    {
        // Check task reward tier
        int baseBonus = task.TaskRewardTier;
        float scaleWithProgressRequired = task.ProgressHoursRequired;
        float randomThrow = UnityEngine.Random.Range(0, 1);
        if (randomThrow > probability)
        {
            return;
        }
        int reward = (int)(baseBonus * scaleWithProgressRequired);
        print($"End of task reward! Gained x{reward} food");
        AccountBalanceAI.UpdateFood(reward);
        // Instantiate the food for drag and drop? => feed cub
        GameObject foodSpawn = GameObject.Instantiate(fodderPrefab, worker.gameObject.transform.position, Quaternion.identity);
        // foodSpawn.transform.SetParent(worker.gameObject.transform);
        fodderPrefab.GetComponent<Fodder>().fodderCount = 1;
        fodderPrefab.GetComponent<Fodder>().feedValue = 50;        
    }
}
