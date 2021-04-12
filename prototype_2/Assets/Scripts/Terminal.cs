using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Terminal : MonoBehaviour
{
    public GameObject terminalCam;
    public GameObject playerMainCamFollow;

    private void OnTriggerEnter(Collider other)
    {
        // If it's the Player, enable their usage of the terminal (make it appear)
        if(other.CompareTag("Player"))
        {
            other.GetComponent<CommandLineController>().commandLineCanvas.GetComponent<Canvas>().enabled = true;
            other.GetComponent<CommandLineController>().commandLineInputField.GetComponent<TMP_InputField>().enabled = true;
            terminalCam.GetComponent<Camera>().enabled = true;
            playerMainCamFollow.GetComponent<Camera>().enabled = false;
            terminalCam.tag = "MainCamera";
            other.transform.LookAt(this.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If it's the Player, enable their usage of the terminal (make it appear)
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CommandLineController>().commandLineCanvas.GetComponent<Canvas>().enabled = false;
            other.GetComponent<CommandLineController>().commandLineInputField.GetComponent<TMP_InputField>().enabled = false;
            terminalCam.GetComponent<Camera>().enabled = false;
            playerMainCamFollow.GetComponent<Camera>().enabled = true;
            terminalCam.tag = "SecondaryCamera";
        }
    }
}
