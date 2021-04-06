using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public sealed class AccountBalanceAI : MonoBehaviour
{
    private const int NB_DAYS_IN_WEEK = 7;
    private const int NB_HOURS_IN_DAY = 12; // Working hours
    private const int NB_SECONDS_IN_HOUR = 3600;
    private const int NB_TICKS_IN_HOUR = 360; // Times account balance is updated    
    public static GameObject moneyUI;
    public static GameObject cubFoodUI;
    public static GameObject taskCountUI;
    public static GameObject workerCountUI;
    public static GameObject cubCountUI;
    public static GameObject gameOverUI;

    // Gameloop
    public static int cubFood; // Needed to feed cubs and therefore train them
    public static float money = 0; // Start at 1000
    public static int taskCount = 0;
    public static int workerCount = 0;
    public static int cubCount = 0;
    public static int totalGain; // Daily
    public static int totalUpcost = 8; // Each 10 seconds -- TODO count it per Building actual costs defined
    public static float netChange = 0;
    // Ledger variables
    public static int foodEarnedToday = 0;
    public static int moneyEarnedToday = 0;
    public static int cubsSold = 0;
    public static int workersTotalExperienceGained = 0;


    private void OnEnable() 
    {        
        SceneController.onClockTicked += UpdateTotalBalance;
    }

    private void OnDisable() 
    {
        SceneController.onClockTicked -= UpdateTotalBalance;
    }

    private void Start()
    {
        moneyUI = GameObject.FindGameObjectWithTag("moneyCount");
        cubFoodUI = GameObject.FindGameObjectWithTag("foodCount");
        gameOverUI = GameObject.FindGameObjectWithTag("gameOverUI");
        taskCountUI = GameObject.FindGameObjectWithTag("taskCount");
        workerCountUI = GameObject.FindGameObjectWithTag("workerCount");
        cubCountUI = GameObject.FindGameObjectWithTag("cubCount");
    }

    // Can be a negative value for withdraw operations
    public static void UpdateMoney(int value) 
    {
        money += value;
        UpdateTotalBalance();
    }

    public static void UpdateFood(int value)
    {
        cubFood += value;
        UpdateTotalBalance();
    }

    public static void UpdateTaskCount(int value)
    {
        taskCount += value;
        UpdateTotalBalance();
    }

    public static void UpdateWorkerCount(int value)
    {
        workerCount += value;
        UpdateTotalBalance();
    }

    public static void UpdateCubCount(int value)
    {
        cubCount += value;
        UpdateTotalBalance();
    }

    public static void UpdateTotalBalance()
    {
        //netChange = (totalGain - (totalUpcost / NB_DAYS_IN_WEEK / NB_HOURS_IN_DAY / NB_SECONDS_IN_HOUR / NB_TICKS_IN_HOUR));
        //netChange = (totalGain - totalUpcost);
        //Debug.Log("Net change: " + netChange);
        //money += netChange;
        UpdateAccountBalanceUI();
    }
    
    public static void UpdateAccountBalanceUI()
    {
        // Update UI
        if(moneyUI != null)
        {
            moneyUI.GetComponent<TextMeshProUGUI>().SetText($"Coin x{money}");
        }
        if(cubFoodUI != null)
        {
            cubFoodUI.GetComponent<TextMeshProUGUI>().SetText($"Food x{cubFood}");
        }
        if(cubCountUI != null)
        {
            cubCountUI.GetComponent<TextMeshProUGUI>().SetText($"Cubs x{cubCount}");
        }
        if(workerCountUI != null)
        {
            workerCountUI.GetComponent<TextMeshProUGUI>().SetText($"Workers x{workerCount}");
        }
        if(taskCountUI != null)
        {
            taskCountUI.GetComponent<TextMeshProUGUI>().SetText($"Tasks x{taskCount}");
        }
    }
}
