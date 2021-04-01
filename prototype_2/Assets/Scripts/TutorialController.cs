using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    // GameObject targets
    public static List<List<List<string>>> conversationGroupsTargets = new List<List<List<string>>>();
    public static List<List<string>> activeConversationGroupTargets = new List<List<string>>();

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
        public List<string> Conversations { get { return conversations; } }
        private List<string> conversations;
        public List<List<string>> ConversationTargets { get { return conversationTargets; } }
        private List<List<string>> conversationTargets;

        public TutorialData(List<string> conversations, List<List<string>> conversationTargets) : this()
        {
            this.conversations = conversations;
            this.conversationTargets = conversationTargets;
        }

        public override string ToString() => $"(Tutorial : )";
    }

    public static void OnConversationEnded()
    {
        Debug.Log("Conversation Ended!");
        int len = System.Enum.GetNames(typeof(Main.TUTORIAL_STATES)).Length;
        ++Main.tutorialState; // This variable is what gives control over conversations in a cluster
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
        List<List<string>> conversationTargets;
        for (int i = 0; i < len; i++)
        {
            // Any conversation action should have a corresponding target for the dialogue action iterator to work
            // Other than that, it's just conversations for display
            conversations = new List<string>();
            conversationTargets = new List<List<string>>();
            switch (i)
            {
                case 0:
                    conversations.Add("Welcome to Momma Cub! I'm the Tutorial Girl, and I'm going to teach you how to play.");
                    conversations.Add("This game is all about managing your Cub farming business. That's right, you're the boss here!");
                    conversations.Add("Also, you were born mute, deaf, paraplegic except for the hands, and nearly blind so you can only speak in programming languages.");
                    conversations.Add("Don't get me wrong. You are the Momma Cub, nonetheless. I will serve you until death follows, and since I don't really exist, that means forever.");
                    conversations.Add("To summarize, your challenge will be to manage your slaves--I mean your workers--to grow your cub empire using only simple programming commands.");
                    conversations.Add("Let's begin!!!");
                    conversationTargets.Add(new List<string>() {""});
                    break;
                case 1:
                    conversations.Add("First, you need to start checking for any new cubs who wish to enroll.");
                    conversations.Add("@[SetGameObjectClickable] ProgramManagement"); // ACTION 0
                    conversations.Add("@[TriggerTextByAlpha] buildingLabel"); // ACTION 1
                    conversations.Add("Click on Program Management now!");
                    conversations.Add("Good job! Now, write 'buy(1, chicken)' in the MommaCub Interface 1.0 input field to buy a stupid chicken!");
                    conversations.Add("@[WaitForCorrectInput] 'buy(1, chicken)'"); // Test
                    conversations.Add("Amazing.");
                    conversations.Add("That felt amazing, master. I am truly your deeply personal interface into this world. ...What did I just say? N-Nothing.");
                    conversations.Add("It goes without saying, but the chicken is somewhere on the map. A camera will zoom on it in a future patch of the game. Anyways, you'll have to trust me it's there. (Check the console log).");
                    conversations.Add("You'll be able to create many more slaves for your benefit, when the game will be completed.");
                    conversations.Add("This is going to be a shocker, but you cannot naturally program things out of existence, if they do not exist in the game.");
                    conversations.Add("At least, not until you find the Supreme Elixir of Momma Creation, hidden away in Level 95995932.");
                    conversations.Add("You don't need to thank me for this. Just remember me when you're that far off in life.");
                    conversations.Add("Anyways, this means only accepted commands will work in the MommaCub Interface. Nothing happens if you write something invalid. Please be gentle.");
                    conversationTargets.Add(new List<string>() { "ProgramManagement", "programManagementLabel" });
                    break;
                case 2:
                    conversations.Add("It's time to visit the Training Centre.");
                    conversations.Add("Exit the Program Management view by pressing Escape. You can always leave buildings by pressing that key.");
                    conversations.Add("@[SetGameObjectClickable] TrainingCentre"); // ACTION 0
                    conversations.Add("@[TriggerTextByAlpha] buildingLabel"); // ACTION 1
                    conversations.Add("Now click on the Training Centre. (You need to do it or you can't proceed, seriously.)");
                    conversations.Add("@[WaitForMouseDown] trainingCentre"); // ACTION 2
                    conversations.Add("This is the most important step, so listen up, Momma.");
                    conversations.Add("END OF DEMO ITERATION ON TUTORIAL AND COMMAND LINE. The rest is just text. It's not implemented yet. The developer lacks sleep, brlergh.");
                    conversations.Add("Your cubs need to eat, rest, and exercise. But YOU choose how often all of these actions happen.");
                    conversations.Add("Any cubs that you put in the feeding pen will feed, so long as there is fodder inside the feeding pen.");
                    conversations.Add("Write 'buy(2, fodder)' to put 2 fodders in the fodder bin. Cubs will automatically try to eat a nearby fodder at each interval of time.");
                    conversations.Add("Any cub that has just eaten will not eat for a while.");
                    conversations.Add("It's evil, but you can also force feed cubs using 'feed()' to feed all cubs, or 'feed(cubnamehere, cubname2here)' to feed one or several cubs.");
                    conversations.Add("By the way, a lot of commands can take the zero sized argument, like 'feed()' or examine(). This just runs your Will on each active cub. Cool, right?");
                    conversations.Add("Cubs that don't eat will starve and will underperform in exercises. Their monetary value will also drop!");
                    conversations.Add("Also, cubs that don't exercise will literally get fatter. Yes, this is a fact.");
                    conversations.Add("The command for exercise is 'exercise(cubnamehere)'");
                    conversations.Add("Is Exercising Good For Cubs? That's either good or bad depending on your client's request. More on that later.");
                    conversations.Add("But if you want to make your cub stronger, faster, and leaner, send them over to combat training exercises!");
                    conversations.Add("Again, this depends on what your client wants. Some prefer fatter cubs, some leaner ones.");
                    conversations.Add("Note that all these actions require you to spend budget money. Lose too much money, and you won't be able to keep up. We'll look at ways that you can make money next.");
                    conversations.Add("To summarize, you need to make sure your cubs eat, rest and exercise depending on your clients' needs. We will look at clients soon, so don't worry.");
                    conversations.Add("Tip: You can see the cubs' hunger, resting and exercise stats at the Resting Lodge by holding the left mouse button over a cub. This will only work at the Resting Lodge! Or, write 'examine(cubnamehere)'");
                    conversationTargets.Add(new List<string>() { "TrainingCentre", "trainingCentreLabel", "OnMouseDown" });
                    break;
                case 3:
                    conversations.Add("Ever wanted to make delicious meat sandwiches? I know I have!");
                    conversations.Add("Exit the Training Centre now by pressing Escape.");
                    conversations.Add("Click on the Slaughterhouse.");
                    conversations.Add("Cubs are cloned for their characteristics and then... you know the 'drill'. What do you think of all this?");
                    conversations.Add("There are three major ways to make money: Making goods out of the cubs at the slaughterhouse, sending cubs to Missions, and selling the cubs themselves. The last option is permanent: you will lose the cub that you sell.");
                    break;
                case 4:
                    conversations.Add("Resting is part of the rules, too. Click on Resting Lodge now.");
                    conversations.Add("Here you can pass time faster and talk with your cubs! You might even receive new mail from sponsors or cubs' relatives...");
                    break;
                case 5:
                    conversations.Add("And finally, you should be aware that clients have specific needs...");
                    conversations.Add("Click on Clients Management now!");
                    break;
                case 6:
                    conversations.Add("Congratulations, you've completed all the tutorials!");
                    conversations.Add("Ahem. You'll have to consult the online API to read all the commands, or wait until the developer adds them in-game. At any rate, we're all glad that the old grab and UI buttons system was ditched unto hell. Seriously, I don't have a clue what to say.");
                    conversations.Add("You're ready to become the most successful cub manager that you can be. Enjoy yourself!");
                    break;
            }
            conversationGroups.Add(conversations);
            conversationGroupsTargets.Add(conversationTargets);
        }
    }

    public static void SetupTutorial()
    {
        // Hide distracting labels
        GameObject.FindGameObjectWithTag("UICanvas").gameObject.GetComponent<Canvas>().enabled = false;
        GameObject[] buildingLabels = GameObject.FindGameObjectsWithTag("buildingLabel");
        // Hide labels for clarity
        foreach (GameObject label in buildingLabels)
        {
            label.GetComponent<TMP_Text>().alpha = 0f;
        }
        activeConversationGroup = conversationGroups[Main.tutorialState];
        activeConversationGroupTargets = conversationGroupsTargets[Main.tutorialState]; // List list string
        TutorialData t = new TutorialData(activeConversationGroup, activeConversationGroupTargets);
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
