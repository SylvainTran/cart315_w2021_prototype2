using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Workfield", menuName = "NavigableLocations/Workfield", order = 0)]
public class Workfield : ScriptableObject {
    public string locationName;
    public Vector3 position;
    public GameObject instance; 
}