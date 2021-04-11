using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public GameObject terminalCam;
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        // If it's the player, enable their usage of the terminal (make it appear)
        if(other.CompareTag("Player"))
        {
            other.GetComponent<CommandLineController>().commandLineCanvas.GetComponent<Canvas>().enabled = true;
            terminalCam.GetComponent<Camera>().enabled = true;
            player.GetComponent<Camera>().enabled = false;
            //player.GetComponent<vThirdPersonCamera>().enabled = false;
            terminalCam.tag = "MainCamera";
            other.transform.LookAt(this.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If it's the player, enable their usage of the terminal (make it appear)
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CommandLineController>().commandLineCanvas.GetComponent<Canvas>().enabled = false;
            terminalCam.GetComponent<Camera>().enabled = false;
            player.GetComponent<Camera>().enabled = true;
            //player.GetComponent<vThirdPersonCamera>().enabled = true;
            terminalCam.tag = "SecondaryCamera";
        }
    }
}
