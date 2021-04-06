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
        print($"Worker has finished work batch.");
        print($"Current working task check 1:{worker.CurrentTask}");
        // Could end before task is completely finished, but here the AI goes to check out the gathered materials to the storehouse
        // Chance reward
        Reward(worker, task, 1f);
        print($"Worker starting new work batch.");
        StopCoroutine("StartNewWorkBatch");
        StartCoroutine(StartNewWorkBatch(worker, task));
    }

    public IEnumerator StartNewWorkBatch(Worker worker, Task task)
    {
        yield return new WaitForSeconds(task.WorkBatchCooldown);
        print($"Worker has a new work batch to work on!");
        worker.CurrentTask.CurrentWorkBatch = 0f;
        worker.IsWorking = true;
        print($"Current working task check 2:{worker.CurrentTask}");
        print($"IS WORKING STATUS: {worker.IsWorking}");
        worker.StartCoroutine("StartWorking");
    }

    public void TaskFinished(Worker worker, Task task)
    {
        // Reward worker and player both!
        Reward(worker, task, 1f);

        // Do other things too
        AccountBalanceAI.UpdateTaskCount(-1);
    }

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
    }
}
