using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameObject tutorialCanvas;    
    private void OnEnable() {
        Main.TutorialController.onTutorialStarted += TriggerTutorialConversation;
    }

    private void OnDisable() {
        Main.TutorialController.onTutorialStarted -= TriggerTutorialConversation;  
    }

    public void Start()
    {
        tutorialCanvas = GameObject.FindGameObjectWithTag("TutorialCanvas");
    }
    public static void TriggerTutorialConversation(Main.TutorialController.TutorialData t)
    {
        Debug.Log($"Tutorial: {t.Conversations[0]}");
        tutorialCanvas.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().SetText(t.Conversations[0]);
    }
}
