using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrepareForSlaughter : MonoBehaviour
{
    public Transform factoryInputConveyorStartPosition;
    public Transform factoryInputDoor;
    
    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.CompareTag("Cub"))
        {
            StripAIComponents(other.gameObject);
            other.GetComponent<Rigidbody>().isKinematic = false;
            PutOnFactoryInputConveyorStartPosition(other.gameObject.transform);
            other.gameObject.transform.LookAt(factoryInputDoor);
        }    
    }

    public void StripAIComponents(GameObject objToStrip)
    {
        if(objToStrip.GetComponent<NavMeshAgent>() != null)
        {
            objToStrip.GetComponent<NavMeshAgent>().enabled = false;
        }
        if(objToStrip.GetComponent<CubAI>() != null)
        {
            objToStrip.GetComponent<CubAI>().enabled = false;
        }
    }

    public void PutOnFactoryInputConveyorStartPosition(Transform objToPlace)
    {
        objToPlace.position = factoryInputConveyorStartPosition.position;
    }
}
