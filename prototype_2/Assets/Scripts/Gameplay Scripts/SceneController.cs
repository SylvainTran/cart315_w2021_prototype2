using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static float timer = 0.0f;
    public static int minutes;
    public static int hours;
    public static int days;
    public static int months;
    public static int years;
    public GameObject minutesClock;
    public GameObject hoursClock;
    public GameObject dayClock;
    private TextMeshProUGUI minutesClockText;
    private TextMeshProUGUI hoursClockText;
    private TextMeshProUGUI dayClockText;
    private float delay = 0.0f;
    private string aMOrPM;
    private string extraText;
    public static float tickInterval = 10.0f;
    public static int minutesIncrementPerTick = 30;
    public GameObject sun;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") {
            return;
        }
        minutesClock = GameObject.FindWithTag("minutesClock");
        minutesClockText = minutesClock.GetComponent<TextMeshProUGUI>();
        hoursClock = GameObject.FindWithTag("hoursClock");
        hoursClockText = hoursClock.GetComponent<TextMeshProUGUI>();
        dayClock = GameObject.FindWithTag("dayClock");
        dayClockText = dayClock.GetComponent<TextMeshProUGUI>();
        days = 0;
        hours = 6; // 6 AM
        minutes = 0;
        // Using invoke repeating method, to avoid repeated polling in update
        InvokeRepeating("UpdateClockTickByInterval", 0f, tickInterval);
    }
    public void FadeToBlack()
    {
        Image img = GetComponentInChildren<Image>();
        Color fixedColor = img.color;
        img.color = fixedColor;
        img.CrossFadeAlpha(1, 2.0f, false);
    }

    public delegate void OnClockTicked();
    public static event OnClockTicked onClockTicked;
    // Event producer
    private void UpdateClockTickByInterval()
    {
        minutes += minutesIncrementPerTick;        
        // Each 60 seconds, add 1 hour
        if(minutes % 60 == 0) {
            minutes = 0;
            hours++;
            Debug.Log("Hour: " + hours);
            if(hours >= 23) {
                Debug.Log("End of day reached");
                dayClockText.SetText($"Season 1: Day {++days}");
                // Show day summary screen
                
                hours = 0;            
            }
        }
        aMOrPM = "PM";
        if(hours < 12) {
            aMOrPM = "AM";
        }
        extraText = "";
        if(minutes == 0) {
            extraText = "0";
        }
        minutesClockText.SetText(extraText + minutes + $" {aMOrPM}");
        hoursClockText.SetText(hours % 12 + ":");  // military format  
        if(hours == 12) {
            hoursClockText.SetText("12:");
        }
        // Update listeners
        Debug.Log("Updating listeners clock tick");
        onClockTicked();
        // Check if lost condition is true (game over if player has a negative balance)
        if (AccountBalanceAI.money <= 0) {
            AccountBalanceAI.gameOverUI.GetComponent<TextMeshProUGUI>().alpha = 255;
            // TODO Show stats
            StartCoroutine(StartChangeScene(3.0f));
        }                  
    }
   
    void Update()
    {
        // TODO move into clockcyclecontroller
        sun.transform.Rotate(0.5f * Time.deltaTime, 0.5f * Time.deltaTime, 0f);
    }

    public static IEnumerator StartChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeScene();
    }
    // Static method for ease
    public static void ChangeScene()
    {
        // There is a bug with unity's scenemanagement methods related to using build index (int)
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SceneManager.LoadScene("MommaCubClub");
        }
        else if (SceneManager.GetActiveScene().name == "MommaCubClub")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
}
