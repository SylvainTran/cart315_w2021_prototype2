using UnityEngine;
using System.Collections;

public class Cub : Character
{
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
    }

    /**
     * Training cost is the cost per training season
     * to develop the cub's stats and talents.
     */
    public int trainingCost;
    /**
     * Cubs perform their professions
     * at a certain level ranging 0-100.
     */
    public int performanceLevel;

    /**
     * Profession of the cub upon graduation.
     */
    public string profession;

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
     * Assets.
     */
    //public Animator animator;
    //public Sprite icon;

    [Header("Sound")]
    public AudioClip complainSound;
    public AudioClip levelUpSound;
    public AudioClip hitSound;
    public AudioClip deathSound;
}
