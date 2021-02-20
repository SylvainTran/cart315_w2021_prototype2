using UnityEngine;
using System.Collections;
using RandomNameGeneratorLibrary;

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

    /**
     * Assets.
     */
    //public Animator animator;
    //public Sprite icon;

    [Header("Sound")]
    public AudioClip complainSound;
    public AudioClip levelUpSound;
    public AudioClip hitSound;
    public AudioClip deathSound;

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
        this.characterVariant = null;
        this.isInTrainingProgram = false;
        this.currentTrainingAction = "IDLE";
    }
}
