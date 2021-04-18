using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrepareForSlaughter : MonoBehaviour
{
    public Transform factoryInputConveyorStartPosition;
    public Transform factoryInputDoor;
    private bool insideTriggerRange = false;
    private bool kinematicIsMoving = false;
    private GameObject enteredGameObject;
    private Coroutine kinematicMovingForwardRoutine;

    private void OnTriggerEnter(Collider other) 
    {
        if(enteredGameObject == null && other.gameObject.CompareTag("Cub"))
        {
            insideTriggerRange = true;
            enteredGameObject = other.gameObject;
            return;
        }
    }

    private void FixedUpdate()
    {
        if(insideTriggerRange)
        {
            StartProcess();
        }
        if(kinematicIsMoving)
        {
            KinematicMoveForward();
        }
    }

    private void StartProcess()
    {
        StripAIComponents(enteredGameObject.gameObject);
        enteredGameObject.GetComponent<Rigidbody>().isKinematic = true;
        PutOnFactoryInputConveyorStartPosition();
        enteredGameObject.gameObject.transform.LookAt(factoryInputDoor);
        insideTriggerRange = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Cub"))
        {
            insideTriggerRange = false;
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

    public void PutOnFactoryInputConveyorStartPosition()
    {
        enteredGameObject.transform.position = factoryInputConveyorStartPosition.position;
        kinematicIsMoving = true;
    }

    public void KinematicMoveForward()
    {
        if(enteredGameObject == null) return;
        enteredGameObject.transform.Translate(Vector3.forward * Time.deltaTime);
    }
}
