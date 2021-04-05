using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RestingSanctuary", menuName = "NavigableLocations/RestingSanctuary", order = 1)]
public class RestingSanctuary : ScriptableObject
{
    public string locationName;
    public Vector3 position;
    public GameObject instance;
}