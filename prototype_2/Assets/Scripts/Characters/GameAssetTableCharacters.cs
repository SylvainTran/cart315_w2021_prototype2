using UnityEngine;
using System.Collections;
using UnityEngine.AddressableAssets;

public class GameAssetTableCharacters : GameAssetTable<Character>
{
    /**
     *  Loads the table.
     */
    public override IEnumerator LoadTable()
    {
        Debug.Log("Asset type label to load: " + "characters");
        yield return Addressables.LoadAssetsAsync<GameObject>("characters", asset =>
        {
            Character c = asset.GetComponent<Character>();
            if (c != null)
            {
                assets.Add(c.GameAssetName, c);
                Debug.Log("Table loaded a " + c.GameAssetName + " with content " + GetAsset(c.GameAssetName));
            }
        });
    }
}
