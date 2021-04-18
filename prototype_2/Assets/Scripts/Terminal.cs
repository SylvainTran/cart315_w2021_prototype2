using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Terminal : MonoBehaviour
{
    public GameObject terminalCam;
    public GameObject playerMainCamFollow;
    public static bool inTerminalRange = false; // disallow flying if in range
    public static GameObject currentTerminal;

    private void OnTriggerEnter(Collider other)
    {
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<FlyBehaviour>().fly)
        {
            print("No fly allowed");
            return;
        }
        // If it's the Player, enable their usage of the terminal (make it appear)
        if(other.CompareTag("Player"))
        {
            currentTerminal = this.gameObject;
            // Focus to prevent move
            //CommandLineController.commandLine.Select();
            CommandLineController.commandLine.ActivateInputField();
            // Reduce speed to remove ice skating
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
            other.GetComponent<CommandLineController>().commandLineCanvas.GetComponent<Canvas>().enabled = true;
            other.GetComponent<CommandLineController>().commandLineInputField.GetComponent<TMP_InputField>().enabled = true;
            terminalCam.GetComponent<Camera>().enabled = true;
            playerMainCamFollow.GetComponent<Camera>().enabled = false;
            terminalCam.tag = "MainCamera";
            other.transform.LookAt(this.transform);
            inTerminalRange = true;
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            CommandLineController.commandLine.Select();
            CommandLineController.commandLine.ActivateInputField();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If it's the Player, enable their usage of the terminal (make it appear)
        if (other.CompareTag("Player"))
        {
            currentTerminal = null;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;            
            UpdateActiveCamera(other.gameObject);
        }
    }

    public void UpdateActiveCamera(GameObject other)
    {
        other.GetComponent<CommandLineController>().commandLineCanvas.GetComponent<Canvas>().enabled = false;
        other.GetComponent<CommandLineController>().commandLineInputField.GetComponent<TMP_InputField>().enabled = false;
        terminalCam.GetComponent<Camera>().enabled = false;
        playerMainCamFollow.GetComponent<Camera>().enabled = true;
        terminalCam.tag = "SecondaryCamera";
        inTerminalRange = false;        
    }
}
