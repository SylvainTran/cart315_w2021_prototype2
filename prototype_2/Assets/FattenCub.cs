using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FattenCub : MonoBehaviour
{
    private void OnTriggerStay(Collider other) 
    {
        if(other.CompareTag("Cub"))
        {
            other.GetComponent<Cub>().startedFattenCub = true;
            other.GetComponent<CubAI>().enabled = false;
            other.GetComponent<NavMeshAgent>().enabled = false;
            other.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Cub"))
        {
            other.GetComponent<Cub>().startedFattenCub = false;
            other.GetComponent<CubAI>().enabled = true;
            other.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
