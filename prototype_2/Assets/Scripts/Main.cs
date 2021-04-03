using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TMPro;
using Cinemachine;

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

    public GameObject mouseSelector; // Used to lift/drag cubs
    public AudioSource cubLiftUpSound;
    public GameObject cubProfileUI; // the menu that shows a cub's statistics
    public GameObject selectedCub;
    public static GameObject cubSpawnSpot;

    public enum GAME_STATES { TUTORIAL, NORMAL, END };
    public enum PLAYER_STATES { MAP, RESTING_LODGE, MENAGERIE, SLAUGHTERHOUSE, PROGRAM_MANAGEMENT, CLIENTS };
    public enum TUTORIAL_STATES { GREETINGS, PROGRAM_MANAGEMENT, MENAGERIE, SLAUGHTERHOUSE, RESTING_LODGE, CLIENTS, COMPLETED_ALL };
    public static int gameState = default;
    public static int playerState = default;
    public static int tutorialState = default;
    public float restartGameDelay = 3.0f;

    // Cams
    public static GameObject globalCam;

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
            int randCubType = UnityEngine.Random.Range(0, 7);
            switch(randCubType) {
                // case 0: cubType = "CatCub"; break;
                case 0: cubType = "chicken"; break;
                case 1: cubType = "cow"; break;
                case 2: cubType = "duck"; break;
                case 3: cubType = "fox"; break;                                
                case 4: cubType = "pig"; break;
                case 5: cubType = "sheep"; break;
                case 6: cubType = "wolf"; break;
                default: cubType = "sheep"; break;
            }
            return (Cub)GameObject.Instantiate(GameAssetsCharacters.GetAsset(cubType), new Vector3(0.638f, 0.2455f, 0.511f), Quaternion.identity);
        }
        public static Cub GenerateNewCubByType(string cubType)
        {
            currentCubRoosterFull = currentCubRooster.Count >= MAX_CUB_CAPACITY ? true : false;
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

        public static IEnumerator GlobalCamPrimacy()
        {
            yield return new WaitForSeconds(3.0f);
            globalCam.GetComponent<CinemachineVirtualCamera>().Priority = 400;
        }

        public static void InitLevel()
        {
            int nbCubs;
            // Assign amount of cubs
            switch(gameState) {
                case 0: nbCubs = 1; break;
                case 1: nbCubs = 10; break;
                case 2: nbCubs = 0; break;
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
        InitResources();
        // Momma Cub Club! Mobile Game
        // TODO LOAD GAME STATE and data from save file
        gameState = (int) GAME_STATES.TUTORIAL;
        playerState = (int) PLAYER_STATES.MAP;
        tutorialState = (int) TUTORIAL_STATES.GREETINGS;
        // Starts the level controller subroutine
        // LevelController.InitLevel();
        // Start tutorials
        LevelController.SetupActors(2);
        TutorialController.InitConversationGroups();
        TutorialController.SetupTutorial();
        onCharactersLoaded();
        globalCam = GameObject.FindGameObjectWithTag("GlobalCam");
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
        Cursor.lockState = CursorLockMode.Confined;       
    }

    /**
    * Current lifted cub and related vars. 
    * TODO make local in scope.
    */
    public GameObject liftedGameObject;
    public NavMeshAgent agent;
    public static Vector3 oldCubPos; // Position before it got lifted up
    public bool grabbedObjectMouseLock = false; // prevent several cubs being lifted
    
    /**
    * Gets a ray to be used in raycasting.
    */
    public Ray GetRay()
    {
        // Move the mouseSelector to the cursor
        Vector3 inputMousePos = Input.mousePosition;
        inputMousePos.z = Camera.main.nearClipPlane * 35;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(inputMousePos);
        mouseSelector.transform.position = mouseWorldPos;
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    /**
    * Raycast Cub characters by tag
    * and updates flags. TODO make these local in scope.
    */
    public RaycastHit RayCastObjects(string tag)
    {
       RaycastHit hit;        
       int layerMask =~ LayerMask.GetMask("TransparentFX");
       if (Physics.Raycast(GetRay(), out hit, Mathf.Infinity, layerMask))
       {
            if(!hit.collider.gameObject.CompareTag(tag))
            {
                liftedGameObject = null;
                grabbedObjectMouseLock = false;
            }
       }
       return hit;
    }

    /**
    * Grabs a Cub character using a raycast hit.
    */
    public void Grab(RaycastHit hit)
    {
        liftedGameObject = hit.collider.gameObject;
        oldCubPos = liftedGameObject.transform.position;

        grabbedObjectMouseLock = true;
        agent = liftedGameObject.GetComponent<NavMeshAgent>();
        if(agent)
        {
            agent.enabled = false;
        }
        liftedGameObject.transform.position = mouseSelector.transform.position;
        liftedGameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
        if(liftedGameObject.gameObject.CompareTag("Cub"))
        {
            if (!cubLiftUpSound.isPlaying)
            {
                cubLiftUpSound.Play();
            }
            string[] fx = { "pickupHeart", "magicalSourceFX" };
            liftedGameObject.GetComponent<Cub>().PlayFXThenDie(fx);
        }
        // TODO highlight cub's outline or material
    }

    public void SelectCub(RaycastHit hit)
    {
        liftedGameObject = hit.collider.gameObject;
        oldCubPos = liftedGameObject.transform.position;
        liftedGameObject.GetComponent<CubAI>().selectedByPlayer = true;
        agent = liftedGameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(Input.mousePosition);
    }

    public bool FilterObjectHit(RaycastHit hit, string tag)
    {
        if(!hit.collider || !hit.collider.gameObject.CompareTag(tag)) {
            return false;
        }
        return true;
    }

    public void ShowCubStats(RaycastHit hit)
    {
        GameObject[] profileUIsOpen = GameObject.FindGameObjectsWithTag("cubProfileUI");
        foreach (GameObject profile in profileUIsOpen)
        {
            if (profile.GetComponent<Canvas>().enabled == true)
            {
                profile.GetComponent<Canvas>().enabled = false;
            }
        }
        hit.collider.gameObject.GetComponent<Cub>().cubProfileUI.GetComponent<UpdateCubProfileUI>().ShowCanvas();

    }

    public GameObject fodderPrefab;

    public void GetFodder()
    {
        Instantiate(fodderPrefab, mouseSelector.transform);
    }

    public void ReturnToMapCameraView()
    {
        CinemachineVirtualCamera[] cams = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
        for (int i = 0; i < cams.Length; i++)
        {
            CinemachineVirtualCamera c = cams[i].GetComponent<CinemachineVirtualCamera>();
            if (c)
            {
                c.Priority = 0;
            }
        }
        globalCam.GetComponent<CinemachineVirtualCamera>().Priority = 200;
        playerState = 0;
    }

    private void Update()
    {
        // Keep holding to open up the cub's profile menu
        if(Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit = RayCastObjects("Cub");
            if(!FilterObjectHit(hit, "Cub"))
            {
                return;
            }
            if (tutorialState >= 4) // Stats can be seen after the Resting Lodge tutorial and above
            {
                ShowCubStats(hit);
            }
        }
        if(Input.GetMouseButton(0)) 
        {
            // Only allow this dragging behaviour in the Menagerie game state
            if(playerState != 2) { // Menagerie State
                return;
            }
            // Grabbing cubs
            RaycastHit cubHit = RayCastObjects("Cub");
            if (FilterObjectHit(cubHit, "Cub"))
            {
                //SelectCub(cubHit);
                Grab(cubHit);
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!grabbedObjectMouseLock)
            {
                return;
            }
            PlaceCharacterOnNavMesh();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMapCameraView();
        }
    }

    /**
    * Replace a Cub in the air on the navmesh
    * to prevent stasis.
    */
    public void PlaceCharacterOnNavMesh()
    {
        if(liftedGameObject) {
            agent.enabled = true;  
            agent.autoRepath = true;
            agent.autoBraking = true;
            agent.speed = 3.5f;          
            agent.Warp(oldCubPos);
        }      
        string[] fx = {"smokePuffFX", "brokenHeartFX"};
        liftedGameObject.GetComponent<Cub>().PlayFXThenDie(fx);            
        grabbedObjectMouseLock = false;
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