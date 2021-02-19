using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public Vector3 respawnPoint;

    private void OnTriggerEnter(Collider collider)
    {
        GameObject collidedObj = collider.gameObject;
        if(collidedObj.CompareTag("Player"))
        {
            collidedObj.transform.position = respawnPoint;
            collidedObj.transform.rotation = Quaternion.identity;
            // Set camera rotation in quaternions from original vector3 rotation
            //Camera.main.transform.rotation = new Vector3(15.45f, -183f, 0f);
        }
    }
}
