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

    public interface IDialogueActionReader
    {
        public void ReadDialogueAction();
    }

    public class DialogueActionReader : IDialogueActionReader
    {
        private string dialogueAction;
        private string actionTargetTag;
        private List<List<string>> conversationGroupsTargets;

        private DialogueActionExecutor DialogueActionExecutor;

        public DialogueActionReader() : this("Show", "Player", null) { }

        public DialogueActionReader(string dialogueAction, string actionTargetTag, List<List<string>> conversationGroupsTargets)
        {
            this.dialogueAction = dialogueAction;
            this.actionTargetTag = actionTargetTag;
            this.conversationGroupsTargets = conversationGroupsTargets;
            DialogueActionExecutor = new DialogueActionExecutor();
        }

        public void ReadDialogueAction()
        {
            switch(dialogueAction)
            {
                case "TriggerTextByAlpha":
                    DialogueActionExecutor.TriggerTextByAlpha(conversationGroupsTargets, actionTargetTag);
                    break;
            }
        }
    }

    public interface IDialogueActionExecutor
    {
        // Set conversation actions to be implemented here
        public void TriggerTextByAlpha(List<List<string>> conversationGroupsTargets, string actionTargetTag);
    }

    public class DialogueActionExecutor : IDialogueActionExecutor
    {
        private int dialogueActionIterator = 0;

        public DialogueActionExecutor() {}

        public void TriggerTextByAlpha(List<List<string>> conversationGroupsTargets, string actionTargetTag)
        {
            if(conversationGroupsTargets[0][dialogueActionIterator].Length <= 0)
            {
                return;
            }
            GameObject[] targetGameObjects = GameObject.FindGameObjectsWithTag(actionTargetTag);
            foreach (GameObject gameObject in targetGameObjects)
            {
                string match = conversationGroupsTargets[0][dialogueActionIterator];
                if (gameObject.name.Equals(match))
                {
                    gameObject.GetComponent<TMP_Text>().alpha = 255.0f;
                    ++dialogueActionIterator;
                }
            }
        }
    }

    private void OnEnable() {
        onTutorialStarted += TriggerTutorialConversation;
    }

    private void OnDisable() {
        onTutorialStarted -= TriggerTutorialConversation;  
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
        // if the current node is a conversation, then display the text
        string dialogueAction;
        string actionTargetTag;
        List<List<string>> activeConversationGroupTargets = new List<List<string>>();
        if (t.Conversations[dialogueNodeIterator][0].Equals('@'))
        {
            dialogueAction = t.Conversations[dialogueNodeIterator].Substring(1, t.Conversations[dialogueNodeIterator].IndexOf("]")).Replace("[", "").Replace("]", "");
            actionTargetTag = t.Conversations[dialogueNodeIterator].Substring(t.Conversations[dialogueNodeIterator].IndexOf(" ") + 1);
            activeConversationGroupTargets = t.ConversationTargets;
            ExecuteDialogueAction(new DialogueActionReader(dialogueAction, actionTargetTag, activeConversationGroupTargets));
        } else
        {
            tutorialCanvas.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().SetText(t.Conversations[dialogueNodeIterator]);
        }
    }

    public static void ExecuteDialogueAction(IDialogueActionReader dialogueActionReader)
    {
        dialogueActionReader.ReadDialogueAction();
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
