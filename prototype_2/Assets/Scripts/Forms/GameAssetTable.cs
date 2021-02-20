using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
* To be contained in a static reference in the GameAssetDatabase
* class. <T> can be a character, form, resource, etc.
*/
public class GameAssetTable<T>
{
    /**
     * Forms used to query the player about in-game
     * mmechanics.
     */
    private static Dictionary<string, T> assets;
    public Dictionary<string, T> Assets { get { return assets; } set { assets = value; } }

    /**
     * Default constructor.
     */
    public GameAssetTable()
    {
        assets = new Dictionary<string, T>();
        Debug.Log("Created new Game Asset Table.");
    }

    /**
     * Get the asset by gameAssetName.
     */
    public static T GetAsset(string gameAssetName)
    {
        Debug.Log("GETTING ASSET !!!!");
        Debug.Log("Query name: " + gameAssetName);
        T a;
        if (assets == null || !assets.TryGetValue(gameAssetName, out a))
        {
            return default(T);
        }
        return a;
    }

    /**
     * Loads database.
     */
    // public abstract IEnumerator LoadTable();
}
