using UnityEngine;
using System.Collections;
using RandomNameGeneratorLibrary;
using UnityEngine.AI;

public class Cub : Character
{
    /**
     * Cubs perform their professions
     * at a certain level ranging 0-100.
     * This is a STAR stat, which means it's used internally
     * to calculate rolls in various areas. In combat,
     * this is combined with training slots/experience slots allocated
     * to combat training actions received in a cub's lifetime.
     */
    public int performanceLevel;
    public int valueRating;
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

    private void Start()
    {
        GameAssetName = "Cub";
        talents = new string[3];
        talents[0] = "Rookie";
        mouseSelector = GameObject.FindGameObjectWithTag("mouseSelector");   
        StartCoroutine(KillFX(0.1f));
    }

    /**
    * ToString override.
    */
    public override string ToString()
    {
        return $"Cub! Name: {characterName}\nVariant: {characterVariant}\nAspect Qualifier: {characterAspectQualifier}\nAggressiveness: {aggressiveness}\nAge: {age}\nLuck: {luck}\nHappiness: {happiness}\nTraining Cost: {trainingCost}\nPerformance Level: {performanceLevel}\nIs In Training Program? {isInTrainingProgram}\nCurrent Training Action: {currentTrainingAction}\n";
    }

    public enum TALENTS { ROOKIE_CHANCE };
    public enum TRAINING_ACTIONS { IDLE };

    /**
    * Generate new stats on this instance of Cub.
    */    
    public void GenerateStats()
    {
        Debug.Log("Generating Stats for this cub.");
        // Base stats
        this.characterName = Utility.GetRandomCharacterFirstName(Random.Range(0, Utility.characterNames.Length));
        this.performanceLevel = 0;
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

    public void PlayLiftFXThenDie()
    {
        ParticleSystem[] childrenParticleSytems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach( ParticleSystem childPS in childrenParticleSytems )
        {
            childPS.Play();
        }
        StartCoroutine(KillFX(2.0f));
    }

    public void PlayDropFXThenDie()
    {
        Debug.Log("In play drop fx then die");
        ParticleSystem[] childrenParticleSytems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach( ParticleSystem childPS in childrenParticleSytems )
        {
            if(childPS.gameObject.CompareTag("smokePuffFX") || childPS.gameObject.CompareTag("brokenHeartFX")) {
                Debug.Log("Found smoke puff fx or broken heart fx");
                childPS.Play();
            }
        }        
        StartCoroutine(KillFX(0.1f));
    }

    public void PlayLevelUpFXThenDie()
    {
        ParticleSystem[] childrenParticleSytems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach( ParticleSystem childPS in childrenParticleSytems )
        {
            if(childPS.gameObject.CompareTag("pickupStarFX")) {
                childPS.Play();
            }
        }        
        StartCoroutine(KillFX(0.1f));  
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
        throw new System.NotImplementedException();
        return base.GetHashCode();
    }
}
