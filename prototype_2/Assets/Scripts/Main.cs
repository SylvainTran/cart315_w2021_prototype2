using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TMPro;

/** 
* Main : MonoBehaviour
*
* Persistent game controller and driver class 
* for global components.
*/
public sealed class Main : MonoBehaviour
{
    public static int MAX_CUB_CAPACITY = 0;
    public static List<Cub> currentCubRooster;
    public static bool currentCubRoosterFull = false;

    public static GameObject cubSpawnSpot;

    public enum GAME_STATES { TUTORIAL, NORMAL, END };
    public enum PLAYER_STATES { MAP, RESTING_LODGE, MENAGERIE, SLAUGHTERHOUSE, PROGRAM_MANAGEMENT, CLIENTS };
    public enum TUTORIAL_STATES { GREETINGS, PROGRAM_MANAGEMENT, MENAGERIE, SLAUGHTERHOUSE, RESTING_LODGE, CLIENTS, COMPLETED_ALL };
    public static int gameState = default;
    public static int playerState = default;
    public static int tutorialState = default;
    public float restartGameDelay = 3.0f;

    /**
    * Character Factory
    * Generates characters on demand.
    */
    public sealed class CharacterFactory
    {
        // Generates a single cub by instantiating the prefab addressed in the GameAssetDatabase
        // Which is hashmapped to the Cub prefab.
        public static Cub GenerateNewCub() 
        {
            string cubType;
            int randCubType = UnityEngine.Random.Range(0, 5);
            switch(randCubType) {
                // case 0: cubType = "CatCub"; break;
                case 0: cubType = "chicken"; break;
                case 1: cubType = "cow"; break;
                case 2: cubType = "duck"; break;
                case 3: cubType = "pig"; break;
                case 4: cubType = "sheep"; break;
                //case 5: cubType = "wolf"; break;
                //case 6: cubType = "fox"; break;
                default: cubType = "sheep"; break;
            }
            AccountBalanceAI.UpdateCubCount(1);
            return (Cub)GameObject.Instantiate(GameAssetsCharacters.GetAsset(cubType), new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        }
        public static Cub GenerateNewCubByType(string cubType)
        {
            currentCubRoosterFull = currentCubRooster.Count >= MAX_CUB_CAPACITY ? true : false;
            AccountBalanceAI.UpdateCubCount(1);
            return (Cub)GameObject.Instantiate(GameAssetsCharacters.GetAsset(cubType), new Vector3(-4.55f, -2.44f, -4.98f), Quaternion.identity);
        }
    }
    // For UI
    public delegate void OnCharactersLoaded();
    public static event OnCharactersLoaded onCharactersLoaded;
    /**
     * Level Controller.
     * Checks current game state and
     * rolls out game subroutines accordingly.
     */
    public sealed class LevelController
    {
        public static void SetupActors(int nbCubs)
        {
            MAX_CUB_CAPACITY = nbCubs;
            currentCubRooster = new List<Cub>(MAX_CUB_CAPACITY);
            // Generate other things
            // ...           
            // GenerateNewCubs(nbCubs);
        }
        public static void GenerateNewCubs(int nbCubs)
        {
            for(int i = 0; i < MAX_CUB_CAPACITY; i++)
            {
                Cub newCub = CharacterFactory.GenerateNewCub();
                newCub.GenerateStats();
                currentCubRooster.Add(newCub);
                Debug.Log(currentCubRooster[i].ToString());
            }            
        }
        public static void GenerateNewCub(int nbCubs, string cubType)
        {
            if(currentCubRooster == null)
            {
                return;
            }
            if(nbCubs + currentCubRooster.Count >= MAX_CUB_CAPACITY)
            {
                Debug.Log("Too many cubs to buy.");
                return;
            }
            Cub newCub = null;
            for (int i = 0; i < nbCubs; i++)
            {
                newCub = CharacterFactory.GenerateNewCubByType(cubType);
                newCub.GenerateStats();
                currentCubRooster.Add(newCub);
                Debug.Log(currentCubRooster[i].ToString());
            }
        }

        public static void InitLevel()
        {
            int nbCubs;
            // Assign amount of cubs
            switch(gameState) {
                case 0: nbCubs = 2; break;
                case 1: nbCubs = 10; break;
                case 2: nbCubs = 15; break;
                default: nbCubs = 10; break;
            }            
            SetupActors(nbCubs);
        }
    }

    public static void InitResources()
    {
        //cubSpawnSpot = Resources.Load<GameObject>("/SpawnSpots/CubSpawnSpot");
        //Instantiate(cubSpawnSpot);
        //Debug.Log(cubSpawnSpot.transform.position);
    }

    public static void LoadMain()
    {
        GameObject main = GameObject.Instantiate(Resources.Load("Initializer")) as GameObject;
        GameObject.DontDestroyOnLoad(main);
        // InitResources();
        // Momma Cub Club! Mobile Game
        // TODO LOAD GAME STATE and data from save file
        gameState = (int) GAME_STATES.NORMAL;
        playerState = (int) PLAYER_STATES.MAP;
        tutorialState = (int) TUTORIAL_STATES.GREETINGS;
        // Starts the level controller subroutine
        LevelController.InitLevel();
        // Start tutorials
        LevelController.SetupActors(10);
        TutorialController.InitConversationGroups();
        TutorialController.SetupTutorial();
        //onCharactersLoaded();
    }

    private void Start()
    {
        GameAssetsCharacters.InitGameAssetsCharacters();
        GameAssetsCharacters.LoadTable();
        LoadMain();
    }
    
    private IEnumerator GameOverState()
    {
        if (gameState == (int)GAME_STATES.END)
        {
            // Game Over
            // Load menu after a while
            yield return new WaitForSecondsRealtime(restartGameDelay);
            SceneManager.LoadScene("MainMenu");
        }
    }
}