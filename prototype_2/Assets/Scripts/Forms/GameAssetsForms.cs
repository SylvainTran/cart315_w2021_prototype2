using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Reflection;

public class GameAssetsForms
{
    /**
     * Forms used to query the player about in-game
     * mmechanics.
     */
    private static Dictionary<string, Form> assets;
    public Dictionary<string, Form> Assets { get { return assets; } set { assets = value; } }

    /**
     * Default constructor.
     */
    public void InitGameAssetsForms()
    {
        assets = new Dictionary<string, Form>();
        Debug.Log("Created new Form Game Asset Table.");
    }

    /**
     * Get the asset by gameAssetName.
     */
    public static Form GetAsset(string gameAssetName)
    {
        Debug.Log("GETTING Form ASSET !!!!");
        Debug.Log("Query name: " + gameAssetName);
        Form a;
        if (assets == null || !assets.TryGetValue(gameAssetName, out a))
        {
            return default(Form);
        }
        return a;
    }
    /**
     *  Loads the table.
     */
    public static IEnumerator LoadTable()
    {
        //Debug.Log("Asset type label to load: " + "forms");
        yield return Addressables.LoadAssetsAsync<GameObject>("forms", asset =>
        {
            Form a = asset.GetComponent<Form>();
            if (a != null)
            {
                // assets.Add(a.GameAssetName, a);
                // Debug.Log("Table loaded a " + a.GameAssetName + " with content " + GetAsset(a.GameAssetName));
            }
        });
    }
}
