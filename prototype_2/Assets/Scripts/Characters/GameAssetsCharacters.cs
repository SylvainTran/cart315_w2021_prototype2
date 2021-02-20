using UnityEngine;
using System.Collections;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

public class GameAssetsCharacters
{
    /**
     * Characters 
     */
    private static Dictionary<string, Character> assets;
    public Dictionary<string, Character> Assets { get { return assets; } set { assets = value; } }

    /**
     * Default constructor.
     */
    public static void InitGameAssetsCharacters()
    {
        assets = new Dictionary<string, Character>();
        Debug.Log("Created new Character Game Asset Table.");
    }

    /**
     * Get the asset by gameAssetName.
     */
    public static Character GetAsset(string gameAssetName)
    {
        Character a;
        if (assets == null || !assets.TryGetValue(gameAssetName, out a))
        {
            return default(Character);
        }
        return a;
    }
    /**
     *  Loads the table.
     */
    public static void LoadTable()
    {
        // Load Cubs prefabs
        assets.Add("Cub", Resources.Load<Character>("Characters/Cub"));
        Debug.Log("Loaded characters table");
    }
}
