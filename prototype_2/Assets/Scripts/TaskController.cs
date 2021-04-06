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
    }

    public void BatchFinished(Worker worker, Task task)
    {
        // Could end before task is completely finished, but here the AI goes to check out the gathered materials to the storehouse
        if(this)
        {
            // Chance reward
            Reward(worker, task, 1f);
            StartCoroutine(StartNewWorkBatch(worker, task));
        }
    }

    public IEnumerator StartNewWorkBatch(Worker worker, Task task)
    {
        yield return new WaitForSeconds(task.WorkBatchCooldown);
        worker.CurrentTask.CurrentWorkBatch = 0f;
        worker.CheckLocationAction();
    }

    public void TaskFinished(Worker worker, Task task)
    {
        // Reward worker and player both!
        Reward(worker, task, 1f);

        // Do other things too
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
