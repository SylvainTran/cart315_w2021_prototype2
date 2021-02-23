using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

/** 
* Main : MonoBehaviour
*
* Persistent driver class for global components.
*/
public sealed class Main : MonoBehaviour
{
    private const int MAX_CUB_CAPACITY = 10;
    public static Cub[] currentCubRooster = new Cub[MAX_CUB_CAPACITY];
    private static bool currentCubRoosterFull = false;
    public GameObject mouseSelector; // Used to lift/drag cubs
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
            int randCubType = Random.Range(0, 7);
            switch(randCubType) {
                // case 0: cubType = "CatCub"; break;
                case 0: cubType = "ChickenCub"; break;
                case 1: cubType = "CowCub"; break;
                case 2: cubType = "DuckCub"; break;
                case 3: cubType = "FoxCub"; break;                                
                case 4: cubType = "PigCub"; break;
                case 5: cubType = "SheepCub"; break;
                case 6: cubType = "WolfCub"; break;
                default: cubType = "SheepCub"; break;
            }
            return (Cub)GameObject.Instantiate(GameAssetsCharacters.GetAsset(cubType), new Vector3(0.638f, 0.2455f, 0.511f), Quaternion.identity);
        }
    }
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
                case 0: break;
                case 1: // GAME 
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
                case 2: // GAME
                    break;
                case 3: // END
                    break;
                default:
                    break;
            }
        }
    }

    public enum GAME_STATES { INTRO, NORMAL, TRAINING_CENTRE, END };
    public static int gameState = default;
    public float restartGameDelay = 3.0f;

    public static void LoadMain()
    {
        GameObject main = GameObject.Instantiate(Resources.Load("Initializer")) as GameObject;
        GameObject.DontDestroyOnLoad(main);

        // Momma Cub Club! Mobile Game
        // TODO LOAD GAME STATE and data from save file
        gameState = (int) GAME_STATES.NORMAL;
        // Starts the level controller subroutine
        LevelController.ApplyCurrentState();
        print("Loaded main");
    }

    private void Start()
    {
        GameAssetsCharacters.InitGameAssetsCharacters();
        GameAssetsCharacters.LoadTable();
        StartCoroutine(GameAssetsForms.LoadTable());
        LoadMain();
        mouseSelector = GameObject.FindGameObjectWithTag("mouseSelector");        
    }

    public GameObject cubLifted;
    public static Vector3 oldCubPos; // Position before it got lifted up
    public bool cubMouseLock = false; // prevent several cubs being lifted
    
    private void Update()
    {
        if(Input.GetMouseButton(0)) 
        {
            //Debug.Log("GAME STATE: " + gameState);
            // Only allow this dragging behaviour in the training centre game state
            if(gameState != 2) { // Training Centre State
                return;
            }
            // Move the mouseSelector to the cursor
            Vector3 inputMousePos = Input.mousePosition;
            inputMousePos.z = Camera.main.nearClipPlane * 14;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(inputMousePos);
            mouseSelector.transform.position = mouseWorldPos;
            //Debug.Log("Mouse selector at: " + mouseSelector.transform.position);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(!hit.collider.gameObject.CompareTag("Cub")) {
                    return;
                }
                // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                // Debug.Log("Did Hit a Cub");
                // Debug.Log("Name: " + hit.collider.gameObject.name);
                cubLifted = hit.collider.gameObject;
                oldCubPos = cubLifted.transform.position;
                cubMouseLock = true;
                //cubLifted.transform.SetParent(mouseSelector.transform);
                cubLifted.GetComponent<NavMeshAgent>().enabled = false;
                cubLifted.transform.position = mouseSelector.transform.position;     
            }
            else
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
                //Debug.Log("Did not Hit");
                cubLifted = null;
                cubMouseLock = false;
            }      
        }
    }

    public void PlaceCharacterOnNavMesh()
    {
        if(cubLifted && !cubMouseLock) {
            cubLifted.GetComponent<NavMeshAgent>().enabled = true;  
            cubLifted.GetComponent<NavMeshAgent>().autoRepath = true;
            cubLifted.GetComponent<NavMeshAgent>().autoBraking = true;
            cubLifted.GetComponent<NavMeshAgent>().speed = 3.5f;          
            cubLifted.GetComponent<NavMeshAgent>().Warp(oldCubPos);
        }    
        Cursor.lockState = CursorLockMode.Confined;   
    }

    private void OnMouseUp() 
    {
        PlaceCharacterOnNavMesh();
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