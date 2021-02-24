using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TMPro;
/** 
* Main : MonoBehaviour
*
* Persistent driver class for global components.
*/
public sealed class Main : MonoBehaviour
{
    public const int MAX_CUB_CAPACITY = 10;
    public static Cub[] currentCubRooster = new Cub[MAX_CUB_CAPACITY];
    private static bool currentCubRoosterFull = false;

    public GameObject mouseSelector; // Used to lift/drag cubs
    public AudioSource cubLiftUpSound;
    public GameObject cubProfileUI; // the menu that shows a cub's statistics

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

    public enum GAME_STATES { INTRO, NORMAL, TRAINING_CENTRE, PROGRAM_MANAGEMENT, CLIENTS, END };
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
        onCharactersLoaded();
        print("Loaded main");
    }

    private void Start()
    {
        GameAssetsCharacters.InitGameAssetsCharacters();
        GameAssetsCharacters.LoadTable();
        StartCoroutine(GameAssetsForms.LoadTable());
        cubLiftUpSound = GetComponent<AudioSource>();
        LoadMain();
        mouseSelector = GameObject.FindGameObjectWithTag("mouseSelector");        
    }

    /**
    * Current lifted cub and related vars. 
    * TODO make local in scope.
    */
    public GameObject cubLifted;
    public NavMeshAgent agent;
    public static Vector3 oldCubPos; // Position before it got lifted up
    public bool cubMouseLock = false; // prevent several cubs being lifted
    
    /**
    * Gets a ray to be used in raycasting.
    */
    private Ray GetRay()
    {
        // Move the mouseSelector to the cursor
        Vector3 inputMousePos = Input.mousePosition;
        inputMousePos.z = Camera.main.nearClipPlane * 14;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(inputMousePos);
        mouseSelector.transform.position = mouseWorldPos;
        return Camera.main.ScreenPointToRay(Input.mousePosition);
        // TODO make a lock for the currently lifted cub, no need to raycast and hit a 
        // collider again on the next frame which causes jittering and fail
        // Also prevent other cubs from being lifted while lifting a cub
    }

    /**
    * Raycast Cub characters by tag
    * and updates flags. TODO make these local in scope.
    */
    private RaycastHit RayCastCharacters()
    {
       RaycastHit hit;        
       if (Physics.Raycast(GetRay(), out hit)){
            {
                if(!hit.collider.gameObject.CompareTag("Cub")) {
                    cubLifted = null;
                    cubMouseLock = false;
                }
            }
        }
        return hit;
    }

    /**
    * Grabs a Cub character using a raycast hit.
    */
    private void GrabCub(RaycastHit hit)
    {
        cubLifted = hit.collider.gameObject;
        oldCubPos = cubLifted.transform.position;
        cubMouseLock = true;
        agent = cubLifted.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        cubLifted.transform.position = mouseSelector.transform.position;
        cubLifted.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));     
        if(!cubLiftUpSound.isPlaying) {
            cubLiftUpSound.Play();
        }
        cubLifted.GetComponent<Cub>().PlayLiftFXThenDie();
        // TODO highlight cub's outline or material
    }

    private bool FilterCubHit(RaycastHit hit)
    {
        if(!hit.collider || !hit.collider.gameObject.CompareTag("Cub")) {
            return false;
        }
        return true;
    }

    private void Update()
    {
        // Keep holding to open up the cub's profile menu
        if(Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit = RayCastCharacters();
            if(!FilterCubHit(hit)) return;
            GameObject[] profileUIsOpen = GameObject.FindGameObjectsWithTag("cubProfileUI");
            foreach(GameObject profile in profileUIsOpen) {
                if(profile.GetComponent<Canvas>().enabled == true) {
                    profile.GetComponent<Canvas>().enabled = false;
                }
            }
            hit.collider.gameObject.GetComponent<Cub>().cubProfileUI.GetComponent<UpdateCubProfileUI>().ShowCanvas();
        }
        if(Input.GetMouseButton(0)) 
        {
            // Only allow this dragging behaviour in the training centre game state
            if(gameState != 2) { // Training Centre State
                return;
            }
            RaycastHit hit = RayCastCharacters();
            if(!FilterCubHit(hit)) return;
            GrabCub(hit);
        }
    }

    /**
    * Replace a Cub in the air on the navmesh
    * to prevent stasis.
    */
    public void PlaceCharacterOnNavMesh()
    {
        if(cubLifted && !cubMouseLock) {
            agent.enabled = true;  
            agent.autoRepath = true;
            agent.autoBraking = true;
            agent.speed = 3.5f;          
            agent.Warp(oldCubPos);
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