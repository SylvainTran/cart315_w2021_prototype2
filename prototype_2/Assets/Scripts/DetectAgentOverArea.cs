using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectAgentOverArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Cub"))
        {
            other.gameObject.GetComponent<CubAI>().ForcePlaceCharacterOnNavMeshPoint();
        }
    }
}
