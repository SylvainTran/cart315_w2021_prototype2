using UnityEngine;
using System.Collections;

public class Character : GameAsset
{
    /**
     * Character sheet.
     */
    public string characterName;
    /**
     * Cubs start with a genetic variant
     * that influences their stat distributions
     * upon leveling up or gaining experience.
     * See GDD for precise definitions.
     */
    public string characterVariant;
    /**
     *  Chars can be HOLY, UNHOLY, SUPER, HEROIC (Cub)
     *  See walkthrough or GDD for precise definitions.
     */
    public int characterAspectQualifier;
    /**
     * Cubs can catch conditions or statuses,
     * like dysentery. This influences
     * their performance.
     */
    public string[] conditions;

    /**
     * Aggressiveness trait. Influences chance of hostility.
     */
    public int aggressiveness;
    /**
     * Age: dies eventually.
     */
    public int age;
    /**
     * Luck: influence rolls and performance.
     */
    public int luck;
    /**
     * Talents: can trigger special bonuses or events.
     */
    public int[] talents;

    /**
     * Happiness: influences performance.
     */
    public int happiness;

    /**
     * Default constructor.
     */
    public Character()
    {

    }

    private void Start()
    {
        GameAssetName = "Character base class";
    }
}
