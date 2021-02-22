using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** 
* Main : MonoBehaviour
*
* Persistent driver class for global components.
*/
public sealed class Main : MonoBehaviour
{
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
            return (Cub)GameObject.Instantiate(GameAssetsCharacters.GetAsset("Cub"), new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
    /**
     * Level Controller.
     * Checks current game state and
     * rolls out game subroutines accordingly.
     */
    public sealed class LevelController
    {
        private const int MAX_CUB_CAPACITY = 10;
        private static Cub[] currentCubRooster = new Cub[MAX_CUB_CAPACITY];
        private static bool currentCubRoosterFull = false;

        public static void ApplyCurrentState()
        {
            switch(gameState)
            {
                case 0: // INTRO 
                    // Setup database
                    // Generate new rooster of cubs using the cub factory
                    // Place cubs at the Resting Lodge building.
                    for(int i = 0; i < MAX_CUB_CAPACITY; i++)
                    {
                        currentCubRooster[i] = CharacterFactory.GenerateNewCub();
                        // Setup cub's random data
                        currentCubRooster[i].GenerateStats();
                        Debug.Log(currentCubRooster[i].ToString());
                        // Add the cub to the building's list of current cubs at
                        currentCubRooster[i].Move("RESTING_LODGE");
                    }
                    break;
                case 1: // GAME
                    break;
                case 2: // END
                    break;
                default:
                    break;
            }
        }
    }

    public enum GAME_STATES { INTRO, GAME, END };
    public static int gameState = default;
    public float restartGameDelay = 3.0f;

    public static void LoadMain()
    {
        GameObject main = GameObject.Instantiate(Resources.Load("Initializer")) as GameObject;
        GameObject.DontDestroyOnLoad(main);

        // Momma Cub Club! Mobile Game
        // TODO LOAD GAME STATE and data from save file
        gameState = (int) GAME_STATES.INTRO;
        // Starts the level controller subroutine
        LevelController.ApplyCurrentState();
        print("Loaded main");
    }

    private void Update()
    {

    }

    private void Start()
    {
        GameAssetsCharacters.InitGameAssetsCharacters();
        GameAssetsCharacters.LoadTable();
        StartCoroutine(GameAssetsForms.LoadTable());
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