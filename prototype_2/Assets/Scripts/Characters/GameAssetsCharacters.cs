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
        // assets.Add("CatCub", Resources.Load<Character>("Characters/CatCub"));
        assets.Add("ChickenCub", Resources.Load<Character>("Characters/ChickenCub"));
        assets.Add("CowCub", Resources.Load<Character>("Characters/CowCub"));
        assets.Add("DuckCub", Resources.Load<Character>("Characters/DuckCub"));
        assets.Add("FoxCub", Resources.Load<Character>("Characters/FoxCub"));
        assets.Add("PigCub", Resources.Load<Character>("Characters/PigCub"));
        assets.Add("SheepCub", Resources.Load<Character>("Characters/SheepCub"));
        assets.Add("WolfCub", Resources.Load<Character>("Characters/WolfCub"));                                                        
        Debug.Log("Loaded characters table");
    }
}
