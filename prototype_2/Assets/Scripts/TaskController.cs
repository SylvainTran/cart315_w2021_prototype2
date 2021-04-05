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
    }

    private void OnDisable() {
        Worker.onTaskFinished += TaskFinished;
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

    public void TaskFinished(Worker worker, Task task)
    {
        // Reward worker and player both!
        Reward(worker, task);

        // Do other things too
    }

    public void Reward(Worker worker, Task task)
    {
        // Check task reward tier
    }
}
