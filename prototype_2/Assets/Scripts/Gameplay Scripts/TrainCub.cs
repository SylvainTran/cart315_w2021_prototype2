using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCub : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Cub"))
        {
            // TODO add particle effect and sound effect!
            //Debug.Log("Cub is training.");
            other.gameObject.GetComponent<Cub>().isInTrainingProgram = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.CompareTag("Cub"))
        {
            //Debug.Log("Cub is leaving training.");
            other.gameObject.GetComponent<Cub>().isInTrainingProgram = false;
        }
    }
}
