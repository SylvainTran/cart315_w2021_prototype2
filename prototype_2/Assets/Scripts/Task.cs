using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask
{
    
}

public class Task : ITask
{
    private string name;
    private static int id;
    private int taskId;
    public int TaskId { get { return taskId; } }    

    private int taskRewardTier = 1; // reward tiers progress in the game
    public int TaskRewardTier { get { return taskRewardTier; } private set { taskRewardTier = value; } }

    private float currentWorkBatchProgress = 0f;
    public float CurrentWorkBatchProgress { get { return currentWorkBatchProgress; } set { currentWorkBatchProgress = value; } }

    // This var is also set by the CLI, the player decides either a fixed amount of task progress required or a fixed amount of hours in-game to work
    private float progressHoursRequired = 8.0f; // this is an arg set by the player
    public float ProgressHoursRequired { get { return progressHoursRequired; } set { if(value > 0) progressHoursRequired = value; } }

    private float workBatchNextTickDelay = 3.0f;
    public float WorkBatchNextTickDelay { get { return workBatchNextTickDelay; } private set { if(value > 0) workBatchNextTickDelay = value; } }    

    private float currentWorkBatch = 0;
    public float CurrentWorkBatch { get { return currentWorkBatch; } set { currentWorkBatch = value; } }

    private float currentWorkBatchLimit = 10.0f;
    public float CurrentWorkBatchLimit { get { return currentWorkBatchLimit; } set { if(value >= 0) currentWorkBatchLimit = value; } }

    public Task()
    {
        ++id;
        taskId = id;
    }

    public override string ToString()
    {
        return $"Task Info: ID: {id}\nReward Tier: {taskRewardTier}\nProgress Hours Required: {progressHoursRequired}\nWork Batch Limit: {currentWorkBatchLimit}\n";
    }
}
