using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrabController : MonoBehaviour
{
    public GameObject grabTransform; // Location where the grabbed object will be repositioned

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cub"))
        {
            Debug.Log($"Contact with {other.gameObject.GetComponent<Cub>().characterName}");
            if(Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log($"Grabbing cub {other.gameObject.GetComponent<Cub>().characterName}");
                other.gameObject.GetComponent<CubAI>().enabled = false;
                other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                other.transform.parent = grabTransform.transform;
                other.transform.position = grabTransform.transform.position;
            }
        }
    }
}
