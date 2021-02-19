using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandleMouse : MonoBehaviour
{
    // Bring canvas associated with it up
    public GameObject buildingMenu;
    private void Start()
    {
    }
    private void OnMouseDown()
    {
        if(!buildingMenu.activeInHierarchy) {
            buildingMenu.SetActive(true);
        }
    }
}
