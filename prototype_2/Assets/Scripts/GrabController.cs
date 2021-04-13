using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrabController : MonoBehaviour
{
     /**
    * Current lifted cub and related vars. 
    * TODO make local in scope.
    */
    public GameObject liftedGameObject;
    public static bool validObjectHovered = false;
    public NavMeshAgent agent;
    public static Vector3 oldCubPos; // Position before it got lifted up
    public static bool grabbedObjectMouseLock = false; // prevent several cubs being lifted
    public GameObject adventureCam;

    public GameObject mouseSelector; // Used to lift/drag cubs
    public GameObject pickupSoundGameObject;
    public AudioSource cubLiftUpSound;
    public GameObject cubProfileUI; // the menu that shows a cub's statistics
    public GameObject selectedCub;

    // Cursors
    public GameObject GameControllerObj;
    private CursorManager CursorManager;

    private void Start()
    {
        mouseSelector = GameObject.FindGameObjectWithTag("mouseSelector"); 
        Cursor.lockState = CursorLockMode.Confined;       
        cubLiftUpSound = pickupSoundGameObject.GetComponent<AudioSource>();
        CursorManager = GameControllerObj.GetComponent<CursorManager>();
    }

    /**
    * Gets a ray to be used in raycasting.
    * Also updates mouse selector's position - TODO refactor
    */
    public Ray GetRay()
    {
        // Move the mouseSelector to the cursor
        Vector3 inputMousePos = Input.mousePosition;
        inputMousePos.z = adventureCam.GetComponent<Camera>().nearClipPlane * 10.25f;
        Vector3 mouseWorldPos = adventureCam.GetComponent<Camera>().ScreenToWorldPoint(inputMousePos);
        //Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouseSelector)
        {
            mouseSelector.transform.position = mouseWorldPos;
        }
        return adventureCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
    }

    /**
    * Raycast Cub characters by tag
    * and updates flags. TODO make these local in scope.
    */
    public RaycastHit RayCastObjects()
    { 
       if(liftedGameObject)
       {
           PlaceCharacterOnNavMesh();   
       }
       RaycastHit hit;        
       int layerMask =~ LayerMask.GetMask("TransparentFX"); //Ignore game objects like the player and the pen colliders

       if (Physics.Raycast(GetRay(), out hit, Mathf.Infinity, layerMask))
       {
           return hit;
       }
       return hit;
    }

    private IEnumerator RevertCursorToDefault(float delay)
    {
        yield return new WaitForSeconds(delay);
        CursorManager.SetDefaultCursor();
    }

    private Vector3 currentColliderSize;
    private float currentCapsuleColliderRadius;
    /**
    * Grabs a Cub character using a raycast hit.
    */
    public void Grab(RaycastHit hit)
    {
        liftedGameObject = hit.collider.gameObject;
        oldCubPos = liftedGameObject.transform.position; // in case the raycast down to reposition to the ground fails, we cache this pos

        grabbedObjectMouseLock = true;
        if(hit.collider.gameObject.CompareTag("Cub"))
        {
            agent = liftedGameObject.GetComponent<NavMeshAgent>();
            if(agent)
            {
                agent.enabled = false;
            }
            string[] fx = { "pickupHeart", "magicalSourceFX" };
            liftedGameObject.GetComponent<Cub>().PlayFXThenDie(fx);
        }

        // liftedGameObject.transform.position = mouseSelector.transform.position;
        // Scale down physics collider temporarily
        currentColliderSize = liftedGameObject.gameObject.GetComponent<BoxCollider>().size;
        liftedGameObject.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.0f, 0.0f, 0.0f);
        currentCapsuleColliderRadius = GetComponent<CapsuleCollider>().radius;
        GetComponent<CapsuleCollider>().radius = 0.0f;
    }

    public void SelectCub(RaycastHit hit)
    {
        liftedGameObject = hit.collider.gameObject;
        oldCubPos = liftedGameObject.transform.position;
        liftedGameObject.GetComponent<CubAI>().selectedByPlayer = true;
        agent = liftedGameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(Input.mousePosition);
    }

    public void SetCursorToObjectHit()
    {
       RaycastHit hit;        
       int layerMask =~ LayerMask.GetMask("TransparentFX");

       if (Physics.Raycast(GetRay(), out hit, Mathf.Infinity, layerMask))
       {
            if(hit.collider.gameObject.CompareTag("Cub") || hit.collider.gameObject.CompareTag("meatProduce"))
            {
                if(!validObjectHovered)
                {
                    hit.collider.gameObject.GetComponent<HighlightController>().HighlightObject();               
                    CursorManager.SetInteractibleCursor(); 
                    hit.collider.gameObject.GetComponent<HighlightController>().StartCoroutine("RemoveHighlightObject", 0.5f);
                    validObjectHovered = true;
                }
            }
            else if(!hit.collider.gameObject.CompareTag("Cub") || !hit.collider.gameObject.CompareTag("meatProduce"))
            {
                CursorManager.SetDefaultCursor();
                validObjectHovered = false;
            } 
       }
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

    private void Update()
    {
        SetCursorToObjectHit();
        // Keep holding to open up the cub's profile menu
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = RayCastObjects();
            if (Main.tutorialState >= 4 || Main.gameState == (int)Main.GAME_STATES.NORMAL) // Stats can be seen after the Resting Lodge tutorial and above
            {
                //ShowCubStats(hit);
            }
        }
        if(Input.GetMouseButton(0)) 
        {
            if(GameModeController.currentGameMode == (int)GameModeController.gameModes.AdventureMode)
            {
                if(liftedGameObject) // If the mouseSelector is anywhere outside the collider of the cub, it will drop automatically
                {
                   GetRay(); // Update mouse selector position
                   liftedGameObject.transform.position = mouseSelector.transform.position;
                   liftedGameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
                   if (!cubLiftUpSound.isPlaying)
                   {
                       cubLiftUpSound.Play();
                   }
                   return;
                }
                RaycastHit cubHit = RayCastObjects();
                if(cubHit.collider != null && (cubHit.collider.gameObject.CompareTag("Cub") || cubHit.collider.gameObject.CompareTag("meatProduce")))
                {
                    Grab(cubHit);
                } else
                {
                    CursorManager.SetErrorCursor();
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!liftedGameObject)
            {
                return;
            }
            liftedGameObject.gameObject.GetComponent<HighlightController>().StartCoroutine("RemoveHighlightObject", 0.3f);
            // Resize physics collider normally
            if(currentColliderSize != null)
            {
                liftedGameObject.gameObject.GetComponent<BoxCollider>().size = currentColliderSize;
            }
            if(currentCapsuleColliderRadius != null)
            {
                GetComponent<CapsuleCollider>().radius = currentCapsuleColliderRadius;
            }
            PlaceCharacterOnNavMesh();
        }
    }

    /**
    * Replace a Cub in the air on the navmesh
    * to prevent stasis.
    */
    public void PlaceCharacterOnNavMesh()
    {
        if(liftedGameObject && agent != null) {
            agent.enabled = true;  
            agent.autoRepath = true;
            agent.autoBraking = true;
            agent.speed = 3.5f; 
            RaycastHit hit;
            int layerMask =~ LayerMask.GetMask("TransparentFX");
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                // if(!hit.collider.gameObject.CompareTag("ground")) {
                //     print("Can't find ground, repositioning at last cached location.");
                //     agent.Warp(oldCubPos - new Vector3(0f, 1f, 0f)); // fallback 
                //     return;
                // }
                agent.Warp(hit.point);
            }         
        }      
        
        
        string[] fx = {"smokePuffFX", "brokenHeartFX"};
        if(liftedGameObject.GetComponent<Cub>())
        {
            liftedGameObject.GetComponent<Cub>().PlayFXThenDie(fx);          
        }
        // Reset  
        grabbedObjectMouseLock = false;
        agent = null;
        liftedGameObject = null;
    }
}
