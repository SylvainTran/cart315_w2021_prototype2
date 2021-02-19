using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour
{
    public float timer = 0.0f;
    public int seconds;
    public GameObject clockTimerUI;
    private TextMeshProUGUI clockTimerUIText;

    private void Start()
    {
        clockTimerUI = GameObject.FindWithTag("ClockTimer");
        clockTimerUIText = clockTimerUI.GetComponent<TextMeshProUGUI>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Template")
        {
            // seconds in float
            timer += Time.deltaTime;
            // turn seconds in float to int
            seconds = (int)(timer % 60);
            //print(seconds);
            clockTimerUIText.SetText(60 - seconds + " seconds.");

            if (60 - seconds <= 1)
            {
                SceneManager.LoadScene("");
            }
        }
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
            SceneManager.LoadScene("Template");
        }
        else if (SceneManager.GetActiveScene().name == "Template")
        {
            SceneManager.LoadScene("Template1");
        }
        else if( SceneManager.GetActiveScene().name == "Template1")
        {
            SceneManager.LoadScene("Template2");
        }
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
}
