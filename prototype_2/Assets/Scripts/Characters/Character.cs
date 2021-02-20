using UnityEngine;
using System.Collections;

public class Character : GameAsset
{
    /**
     * Character sheet.
     */
    public string characterName;
    /**
     * Training a cub will randomly unlock a certain variant.
     * A desired variant can be "freezed" and trained upon, or reset.
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
