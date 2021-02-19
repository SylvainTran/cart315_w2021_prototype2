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
     * Level Controller.
     * Checks current game state and
     * rolls out game subroutines accordingly.
     */
    public sealed class LevelController
    {
        public static void ApplyCurrentState()
        {
            switch(gameState)
            {
                case 0: // INTRO                    
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

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain()
    {
        GameObject main = GameObject.Instantiate(Resources.Load("Initializer")) as GameObject;
        GameObject.DontDestroyOnLoad(main);

        // Momma Cub! Mobile Game
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
