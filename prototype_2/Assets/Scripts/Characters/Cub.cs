using UnityEngine;
using System.Collections;
using RandomNameGeneratorLibrary;
using UnityEngine.AI;

public class Cub : Character
{
    public int leanness = 50;
    public int valueRating = 150;
    public int tierRewards = 1; // scale factor when getting meat or produce -- progressable var

    /**
     * Cub's current hunger. Decreased by eating food
     */
    private float satiety = 50;
    public float Satiety { get { return satiety; } set { if (value > 0) satiety = value; } }

    /**
     * Talents: the player "gambles" to get a combination of 1-3
     * talents. Frozen upon graduating. The higher the performance, the
     * higher grade of talent.
     */
    public string[] talents;

    /**
     * "Acquired Experience Slots" are selected experiences that influences
     * a cub's preferences in life and their rolls. Max 10.
     */

    public string[] acquiredExperienceSlots;
    /**
     * Training cost is the cost per training season
     * to develop the cub's stats and talents.
     */

    public int trainingCost;
    /**
     * Whether the cub is currently in a training program.
     */
    public bool isInTrainingProgram = false;

    /**
     * Cub's current training action alloted
     * by the player.
     */
    public string currentTrainingAction;

    /**
    * Is this cub in the player's mouse selection?
    */
    public bool isMouseSelected = false;
    public GameObject mouseSelector; // Used to lift/drag cubs    
    /**
    * Current building at.
    */
    public string currentBuildingAt;
    
    /**
    * Move event
    */ 
    public delegate void CharacterMoved(GameObject c);
    public static event CharacterMoved OnCharacterMoved;

    /**
     * Assets.
     */
    //public Animator animator;
    //public Sprite icon;

    [Header("Sound and FX")]
    public AudioSource cubLiftUpSound;

    [Header("UI")]
    public GameObject cubProfileUI;

    public int MAX_BASE_CHARACTER_POINTS = 25;
    /**
     * Default constructor.
     * Adds to the static game id.
     */
    public Cub()
    {

    }

    private void OnEnable()
    {
        SceneController.onClockTicked += OnClockTicked;
    }

    private void OnDisable()
    {
        SceneController.onClockTicked -= OnClockTicked;
    }

    private void Awake()
    {
        StartCoroutine(KillFX(0.1f));
    }

    private void Start()
    {
        GameAssetName = "Cub";
        talents = new string[3];
        talents[0] = "Rookie";
        mouseSelector = GameObject.FindGameObjectWithTag("mouseSelector");   
    }

    /**
     * Decrease satiety every clock tick
     */
    public void OnClockTicked()
    {
        satiety -= 0.5f;
        if(satiety <= 0.0f)
        {
            satiety = 0.0f;
            print("Cub is nearing oblivion from not eating");
            --valueRating; // permanently lose value rating
        }
        if(startedFattenCub)
        {
            fatten();
        } else if(isInTrainingProgram)
        {
            train();
        }
    }

    /**
    * ToString override.
    */
    public override string ToString()
    {
        return $"Cub! Name: {characterName}\nVariant: {characterVariant}\nAspect Qualifier: {characterAspectQualifier}\nAggressiveness: {aggressiveness}\nAge: {age}\nLuck: {luck}\nHappiness: {happiness}\nTraining Cost: {trainingCost}\nLeanness: {leanness}\nIs In Training Program? {isInTrainingProgram}\nCurrent Training Action: {currentTrainingAction}\n";
    }

    public enum TALENTS { ROOKIE_CHANCE };
    public enum TRAINING_ACTIONS { IDLE };

    /**
    * Generate new stats on this instance of Cub.
    */    
    public void GenerateStats()
    {
        //Debug.Log("Generating Stats for this cub.");
        // Base stats
        this.characterName = Utility.GetRandomCharacterFirstName(Random.Range(0, Utility.characterNames.Length));
        this.leanness = 10;
        this.valueRating = 150;
        this.age = 1;
        this.aggressiveness = Random.Range(0, 10);
        this.luck = Random.Range(0, 10);    
        this.happiness = 100;

        // Training
        this.isInTrainingProgram = false;
        this.currentTrainingAction = "IDLE";
    }

    /**
    * Move to the specified location internally by setting a flag. This is 
    * an event that buildings listen to. Note, this is not a behaviour but a model operation.
    * The behaviours are on the CubAI component instead.
    */
    public void Move(string location)
    {
        currentBuildingAt = location;
        Debug.Log("Move to: " + currentBuildingAt);
        // fire move event to OnMove Listeners on buildings
        if(OnCharacterMoved != null) {
            OnCharacterMoved(this.gameObject);
        }
    }

    public void PlayFXThenDie(string targetTag)
    {
        ParticleSystem[] childrenParticleSytems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach( ParticleSystem childPS in childrenParticleSytems )
        {
            if(childPS.gameObject.CompareTag(targetTag)) {
                childPS.Play();
            }
        }
        StartCoroutine(KillFX(2.0f));
    }
    // Overload for multiple tags
    public void PlayFXThenDie(string[] targetTags)
    {
        ParticleSystem[] childrenParticleSytems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach( ParticleSystem childPS in childrenParticleSytems )
        {
            foreach(string t in targetTags)
            {
                if(childPS.gameObject.CompareTag(t)) {
                    childPS.Play();
                }                
            }
        }
        StartCoroutine(KillFX(2.0f));
    }

    private IEnumerator KillFX(float delay)
    {
        yield return new WaitForSeconds(delay);
        ParticleSystem[] childrenParticleSytems = gameObject.GetComponentsInChildren< ParticleSystem >();
        foreach( ParticleSystem childPS in childrenParticleSytems )
        {
            childPS.Stop();
        }
    }

    public override bool Equals(object obj)
    {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }        
        Cub c = (Cub) obj;
        return characterName == (c.characterName); // TODO add UUID for collision safety
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        return base.GetHashCode();
    }

    public bool startedFattenCub = false;
    public void fatten()
    {
        valueRating = valueRating + (leanness - (--leanness));
        if(leanness <= 0) leanness = 0;
        if(valueRating <= 0) valueRating = 0;
        this.transform.localScale += new Vector3(0.10f, 0.10f, 0.10f);
        if(this.transform.localScale.x >= 3.5f)
        {
            this.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        }
        // TODO fatness is good for cub's produces
    }

    public void train()
    {
        // leanness is good for cub's meat 
        valueRating = valueRating + (leanness - (++leanness));
        if(leanness >= 100) leanness = 100;
        if(valueRating <= 0) valueRating = 0;
        this.transform.localScale -= new Vector3(0.10f, 0.10f, 0.10f);
        if(this.transform.localScale.x <= 1.0f)
        {
            this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
     }
}
