using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    public Material defaultMaterial;
    public Material highlightMaterial;
    public Material[] mats;
    private MeshRenderer rend;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
        mats = new Material[0];
        mats = rend.materials;
        defaultMaterial = mats[0];
    }

    // private void OnTriggerEnter(Collider collider)
    // {
    //     if(rend == null)
    //     {
    //         return;
    //     }
    //     if (collider.gameObject.CompareTag("Player"))
    //     {
    //         print("Highlight with the player");
    //         HighlightObject();
    //     }
    // }

    // private void OnTriggerExit(Collider collider)
    // {
    //     if(collider.gameObject.CompareTag("Player"))
    //     {
    //         RemoveHighlightObject();
    //     }
    // }

    public void HighlightObject()
    {
        if(rend == null)
        {
            return;
        }
        // Swap to highlight mat at the second index
        mats[0] = highlightMaterial;
        //mats[mats.Length-1] = highlightMaterial;
        rend.materials = mats;
    }

    public IEnumerator RemoveHighlightObject(float delay)
    {
        yield return new WaitForSeconds(delay);
        mats[0] = defaultMaterial;
        //mats[mats.Length - 1] = null;
        rend.materials = mats;
    }
}
