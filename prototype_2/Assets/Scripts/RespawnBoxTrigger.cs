using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBoxTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}
