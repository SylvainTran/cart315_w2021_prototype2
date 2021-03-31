using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TutorialController;

// TODO rename to conversation controller or something
public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameObject tutorialCanvas;
    public static TutorialData tutorialData;
    public static int dialogueNodeIterator = 0;
    public delegate void ConversationFlowEnded();
    public static event ConversationFlowEnded onConversationFlowEnded;

    private void OnEnable() {
        onTutorialStarted += TriggerTutorialConversation;
    }

    private void OnDisable() {
        onTutorialStarted -= TriggerTutorialConversation;  
    }

    public static void TriggerTutorialConversation(TutorialData t)
    {
        tutorialCanvas = GameObject.FindGameObjectWithTag("TutorialCanvas");
        tutorialData = t;
        RefreshDialogueFlow(tutorialData);
    }

    public static void RefreshDialogueFlow(TutorialData t)
    {
        tutorialCanvas.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().SetText(t.Conversations[dialogueNodeIterator]);
    }

    public void ContinueDialogueFlow()
    {
        if(Main.tutorialState == (int) Main.TUTORIAL_STATES.COMPLETED_ALL)
        {
            return;
        }
        // If has next conversation groups then proceed not
        if(TutorialController.conversationGroups[Main.tutorialState + 1][0].Length == 0)
        {
            return;
        }
        if (ConversationEnded())
        {
            // Then start next tutorial or conversation flow
            //if (onConversationFlowEnded != null)
            //{
            //    dialogueNodeIterator = 0;
            //    onConversationFlowEnded();
            //}
            dialogueNodeIterator = 0;
            TutorialController.OnConversationEnded();
            return;
        }
        ++dialogueNodeIterator;
        int max = tutorialData.Conversations.Count - 1;
        dialogueNodeIterator = Mathf.Clamp(dialogueNodeIterator, 0, max);
        RefreshDialogueFlow(tutorialData);
    }

    public bool ConversationEnded()
    {
        return dialogueNodeIterator == tutorialData.Conversations.Count - 1;
    }
}
