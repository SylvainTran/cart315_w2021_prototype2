using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Controls tutorial flows.
*/
public class TutorialController : MonoBehaviour
{
    /**
    * Tutorial start event
    */
    public delegate void TutorialStarted(TutorialData t);
    public static event TutorialStarted onTutorialStarted;
    public static List<List<string>> conversationGroups = new List<List<string>>();
    public static List<string> activeConversationGroup = new List<string>();

    private void OnEnable()
    {
        UIController.onConversationFlowEnded += OnConversationEnded;
    }

    private void OnDisable()
    {
        UIController.onConversationFlowEnded -= OnConversationEnded;
    }

    public struct TutorialData
    {
        public TutorialData(List<string> conversations, Building building) : this()
        {
            this.Conversations = conversations;
            this.Building = building;
        }

        public List<string> Conversations { get; }
        private List<string> conversations;
        public Building? Building { get; }
        private Building? building;

        public override string ToString() => $"(Tutorial : )";
    }

    public static void OnConversationEnded()
    {
        Debug.Log("Conversation Ended!");
        int len = System.Enum.GetNames(typeof(Main.TUTORIAL_STATES)).Length;
        ++Main.tutorialState;
        if(Main.tutorialState >= len)
        {
            Main.tutorialState = (int)Main.TUTORIAL_STATES.COMPLETED_ALL;
        }
        // if there are remaining conversation groups, advance to the next one
        SetupTutorial();
    }

    public static void InitConversationGroups()
    {
        int len = System.Enum.GetNames(typeof(Main.TUTORIAL_STATES)).Length;
        List<string> conversations;
        for (int i = 0; i < len; i++)
        {
            conversations = new List<string>();
            switch (i)
            {
                case 0:
                    conversations.Add("Welcome to Momma Cub! I'm the Tutorial Girl, and I'm going to teach you how to play.");
                    conversations.Add("This game is all about managing your Cub Academy. That's right, you're the boss here!");
                    conversations.Add("Let's begin!!!");
                    break;
                case 1:
                    conversations.Add("First, you need to start checking for any new cubs who wish to enroll.");
                    conversations.Add("Click on Program Management now!");
                    ShowAParticularGameObject("programManagementLabel");
                    conversations.Add("Good job! Now click on any of the names of the cubs that you wish to admit.");
                    break;
                case 2:
                    conversations.Add("It's time to visit the Training Centre.");
                    conversations.Add("Exit the Program Management view and click on Training Centre now!");
                    ShowAParticularGameObject("trainingCentreLabel");
                    conversations.Add("This is the most important step, so listen up.");
                    conversations.Add("Your cubs need to eat, rest, and exercise. But YOU choose how often all of these actions happen.");
                    conversations.Add("Any cubs that you put in the feeding pen will feed, so long as there is fodder inside the feeding pen.");
                    conversations.Add("Drag and drop fodder from the fodder bin to the pen to put it there. Cubs will automatically try to eat a nearby fodder at an interval of time.");
                    conversations.Add("Any cub that has just eaten will not eat for a while.");
                    conversations.Add("Cubs that don't eat will starve and will underperform in exercises. Their monetary value will also drop!");
                    conversations.Add("Also, cubs that don't exercise will literally get fatter. Yes, this is a fact.");
                    conversations.Add("That's either good or bad depending on your client's request. More on that later.");
                    conversations.Add("But if you want to make your cub stronger, faster, and leaner, send them over to combat training exercises!");
                    conversations.Add("Again, this depends on what your client wants. Some prefer fatter cubs, some leaner ones.");
                    conversations.Add("Note that all these actions require you to spend budget money. Lose too much money, and you won't be able to keep up. We'll look at ways that you can make money next.");
                    conversations.Add("To summarize, you need to make sure your cubs eat, rest and exercise depending on your clients' needs. We will look at clients soon, so don't worry.");
                    break;
                case 3:
                    conversations.Add("Ever wanted to make delicious meat sandwiches? I know I have!");
                    conversations.Add("Exit the Training Centre now and click on the Slaughterhouse building.");
                    conversations.Add("Cubs are cloned for their characteristics and then... you know the 'drill'. What do you think of all this?");
                    conversations.Add("There are three major ways to make money: Making goods out of the cubs at the slaughterhouse, sending cubs to Missions, and selling the cubs themselves. The last option is permanent: you will lose the cub that you sell.");
                    break;
                case 4:
                    conversations.Add("Make sure to rest sometimes. You'll work better that way. Click on Resting Lodge now.");
                    conversations.Add("Here you can pass time faster and talk with your cubs! You might even receive new mail from sponsors or cubs' relatives...");
                    break;
                case 5:
                    conversations.Add("And finally, you should be aware that clients have specific needs...");
                    conversations.Add("Click on Clients Management now!");
                    break;
                case 6:
                    conversations.Add("Congratulations, you've completed all the tutorials!");
                    conversations.Add("You're ready to become the most successful cub manager that you can be. Enjoy yourself!");
                    break;
            }
            conversationGroups.Add(conversations);
        }
    }

    public static void ShowAParticularGameObject(string gameObjectName)
    {
        GameObject[] buildingLabels = GameObject.FindGameObjectsWithTag("buildingLabel");
        foreach (GameObject label in buildingLabels)
        {
            if(label.gameObject.name.Equals(gameObjectName)) {
                label.SetActive(true);
            }
        }
    }

    public static void SetupTutorial()
    {
        // Hide distracting labels
        GameObject.FindGameObjectWithTag("UICanvas").gameObject.GetComponent<Canvas>().enabled = false;
        GameObject[] buildingLabels = GameObject.FindGameObjectsWithTag("buildingLabel");
        foreach (GameObject label in buildingLabels)
        {
            label.SetActive(false);
        }
        activeConversationGroup = conversationGroups[Main.tutorialState];
        TutorialData t = new TutorialData(activeConversationGroup, null);
        RunTutorial(t);
    }

    public static void RunTutorial(TutorialData t)
    {
        // Trigger event to canvas handler with the conversations
        UIController.TriggerTutorialConversation(t);
    }

    public static void TeardownTutorial()
    {
    }
}
