using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Reflection;

public class GameAssetTableForms : GameAssetTable<Form>
{
    /**
     *  Loads the table.
     */
    public override IEnumerator LoadTable()
    {
        Debug.Log("Asset type label to load: " + "forms");
        yield return Addressables.LoadAssetsAsync<GameObject>("forms", asset =>
        {
            Form a = asset.GetComponent<Form>();
            if (a != null)
            {
                assets.Add(a.GameAssetName, a);
                Debug.Log("Table loaded a " + a.GameAssetName + " with content " + GetAsset(a.GameAssetName));
            }
        });
        // Load up the references to be used for instantiation by other classes

        // Action Forms
        //Form creationForm;
        //int len = assets.Count;
        //Debug.Log("Forms Count: " + len);
        //for (int i = 0; i < len; i++)
        //{
        //    //GetAsset("AcademyCreationForm", out creationForm);
        //    // Test
        //    Debug.Log("GAME OBJECT: " + creationForm.thisGameObject);
        //    // Trigger OnLoaded event? => listener in Level Controller/Main?
        //     Instantiate the loadded forms
        //     Attach onClick.AddListener(() => action(index)); on the submit button
        //     validate the input fields before
        //}

    }
}
