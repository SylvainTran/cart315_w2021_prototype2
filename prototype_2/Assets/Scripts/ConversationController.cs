using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TutorialController;
using System;

public class ConversationController : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameObject tutorialCanvas;
    public static TutorialData tutorialData;
    public static int dialogueNodeIterator = 0;
    public static int dialogueActionIterator = 0;
    public static bool pauseConversations = false;
    public static bool iteratorTriggeredLastActionYet = false;
    public delegate void ConversationFlowEnded();
    public static event ConversationFlowEnded onConversationFlowEnded;
    public static DialogueActionExecutor dialogueActionExecutor = new DialogueActionExecutor();
    public GameObject TRAINING_CENTRE;
    public GameObject PROGRAM_MANAGEMENT;

    public interface IDialogueActionExecutor
    {
        // Set all conversation actions to be implemented here
        public void ReadDialogueAction();
        public void TriggerTextByAlpha();
        public void SetGameObjectClickable();
        public void WaitForMouseDown();
        public void SetAction(string dialogueAction, string actionTargetTag, List<List<string>> activeConversationGroupTargets);
    }

    public class DialogueActionExecutor : IDialogueActionExecutor
    {
        private string dialogueAction;
        private string actionTargetTag;
        public string ActionTargetTag { get { return actionTargetTag;  } }
        private List<List<string>> conversationGroupsTargets;

        public DialogueActionExecutor() : this(null, null, null) { }
        public DialogueActionExecutor(string dialogueAction, string actionTargetTag, List<List<string>> conversationGroupsTargets)
        {
            this.dialogueAction = dialogueAction;
            this.actionTargetTag = actionTargetTag;
            this.conversationGroupsTargets = conversationGroupsTargets;
        }

        public void ReadDialogueAction()
        {
            switch (dialogueAction)
            {
                case "TriggerTextByAlpha":
                    TriggerTextByAlpha();
                    break;
                case "SetGameObjectClickable":
                    SetGameObjectClickable();
                    break;
                case "WaitForMouseDown":
                    WaitForMouseDown();
                    break;
                case "WaitForCorrectInput":
                    WaitForCorrectInput();
                    break;
                default:
                    break;
            }
        }

        public void SetAction(string dialogueAction, string actionTargetTag, List<List<string>> conversationGroupsTargets)
        {
            this.dialogueAction = dialogueAction;
            this.actionTargetTag = actionTargetTag;
            this.conversationGroupsTargets = conversationGroupsTargets;
            print($"setting action {dialogueAction}\nactiontargettag {actionTargetTag}\nconversationgrouptargets {conversationGroupsTargets}");
        }

        public void TriggerTextByAlpha()
        {
            if(conversationGroupsTargets[0] == null)
            {
                return;
            }
            GameObject conversationTarget = FindConversationTarget();
            if (conversationTarget)
            {
                conversationTarget.GetComponent<TMP_Text>().alpha = 255.0f;
                ++dialogueActionIterator;
            } else
            {
                throw new NullReferenceException("Conversation object not found! Check the tags again or wait until the developer gets good, and adds addressable assets to preload. What a moron!");
            }
        }

        public void SetGameObjectClickable()
        {
            GameObject conversationTarget = FindConversationTarget();
            conversationTarget.GetComponent<Building>().interactibleState = true;
            ++dialogueActionIterator;
        }

        public void WaitForCorrectInput()
        {
            pauseConversations = true;
            CommandLineController.commandLine.onSubmit.AddListener((data) => { ValidateCurrentInput(data); });
        }

        public void ValidateCurrentInput(string data)
        {
            if (data.Length == 0)
            {
                return;
            }
            print($"Submit data: {data}");
            print($"Required data: {actionTargetTag}");
            if (data.Trim() == actionTargetTag.Trim())
            {
                print($"We've got a winner! Unpausing flow.");
                ++dialogueActionIterator;
                pauseConversations = false;
                CommandLineController.commandLine.onSubmit.RemoveAllListeners();
                RefreshDialogueFlow(tutorialData);
            }
        }

        public void WaitForMouseDown()
        {
            pauseConversations = true;
            print("Paused conversation flow");
            CustomEventController.eventLookedFor = "ValidateOnMouseDownTarget";
        }

        public IEnumerator WaitForMouseDownSeconds(float delay)
        {
            yield return new WaitForSeconds(delay);
        }

        public GameObject FindConversationTarget()
        {
            GameObject[] targetGameObjects = GameObject.FindGameObjectsWithTag(actionTargetTag);
            foreach (GameObject gameObject in targetGameObjects)
            {
                string match = conversationGroupsTargets[0][dialogueActionIterator];
                if (gameObject.name.Equals(match))
                {
                    return gameObject;
                }
            }
            return null;
        }
    }

    private void OnEnable() {
        TutorialController.onTutorialStarted += TriggerTutorialConversation;
    }

    private void OnDisable() {
        TutorialController.onTutorialStarted -= TriggerTutorialConversation;  
    }

    public static void TriggerGameObject(string gameObjectName)
    {

    }

    public static void TriggerTutorialConversation(TutorialData t)
    {
        tutorialCanvas = GameObject.FindGameObjectWithTag("TutorialCanvas");
        tutorialData = t;
        RefreshDialogueFlow(tutorialData);
    }

    public static void RefreshDialogueFlow(TutorialData t)
    {
        if (ConversationEnded())
        {
            dialogueNodeIterator = 0;
            dialogueActionIterator = 0;
            //TutorialController.OnConversationEnded();
            onConversationFlowEnded();
            return;
        }
        if(pauseConversations)
        {
            return;
        }
        // if the current node is a conversation, then display the text
        string dialogueAction;
        string actionTargetTag;
        List<List<string>> activeConversationGroupTargets = new List<List<string>>();
        if (t.Conversations[dialogueNodeIterator][0].Equals('@'))
        {
            dialogueAction = t.Conversations[dialogueNodeIterator].Substring(1, t.Conversations[dialogueNodeIterator].IndexOf("]")).Replace("[", "").Replace("]", "");
            print(dialogueAction);
            actionTargetTag = t.Conversations[dialogueNodeIterator].Substring(t.Conversations[dialogueNodeIterator].IndexOf(" ") + 1);
            print(actionTargetTag);
            activeConversationGroupTargets = t.ConversationTargets;
            //print(activeConversationGroupTargets);
            // Only run actions if we haven't reached the end of the actions group yet
            if(dialogueActionIterator < t.ConversationTargets[0].Count)
            {
                dialogueActionExecutor.SetAction(dialogueAction, actionTargetTag, activeConversationGroupTargets);
                ExecuteDialogueAction(dialogueActionExecutor);
            }
            // Skip to the next conversation node
            ContinueDialogueFlow();
        } else if(t.Conversations[dialogueNodeIterator][0].Equals('#'))
        {
            // Text preceding 
            if (tutorialCanvas)
            {
                tutorialCanvas.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().SetText(t.Conversations[dialogueNodeIterator]);
                ++dialogueNodeIterator;
                RefreshDialogueFlow(t);
            }
        }
        else
        {
            if(tutorialCanvas) {
                tutorialCanvas.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().SetText(t.Conversations[dialogueNodeIterator]);
            } 
        }
    }

    public void LateUpdate()
    {
        if(pauseConversations == true)
        {
            return;
        }
    }

    // TODO change into event driven (make executors listeners)
    public static void ExecuteDialogueAction(IDialogueActionExecutor dialogueActionExecutor)
    {
        dialogueActionExecutor.ReadDialogueAction();
    }

    public static void ContinueDialogueFlow()
    {
        if(Main.tutorialState == (int) Main.TUTORIAL_STATES.COMPLETED_ALL)
        {
            return;
        }
        if(pauseConversations)
        {
            print("Returning because conversation is paused");
            return;
        }        
        // If has next conversation groups then proceed not
        if(TutorialController.conversationGroups[Main.tutorialState + 1][0].Length == 0)
        {
            return;
        }
        if (ConversationEnded())
        {
            dialogueNodeIterator = 0; // This variable needs to be reset so that the next conversation group starts at its beginning
            dialogueActionIterator = 0;
            //TutorialController.OnConversationEnded();
            onConversationFlowEnded();
            return;
        }
        ++dialogueNodeIterator;
        int max = tutorialData.Conversations.Count - 1;
        dialogueNodeIterator = Mathf.Clamp(dialogueNodeIterator, 0, max);
        RefreshDialogueFlow(tutorialData);
    }

    public static bool ConversationEnded()
    {
        return dialogueNodeIterator == tutorialData.Conversations.Count - 1;
    }
}
