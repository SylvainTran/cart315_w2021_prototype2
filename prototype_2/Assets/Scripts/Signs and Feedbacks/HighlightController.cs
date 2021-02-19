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
        mats = new Material[2];
        mats = rend.materials;
        defaultMaterial = mats[0];
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player")) {
            return;
        }
        // Swap to highlight mat at the second index
        mats[1] = highlightMaterial;
        rend.materials = mats;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
        {
            return;
        }
        mats[0] = defaultMaterial;
        mats[1] = null;
        rend.materials = mats;
    }
}
