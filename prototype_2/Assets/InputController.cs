using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;

public class InputController : MonoBehaviour
{
    private void OnEnable()
    {
        CommandLineController.onCommandLineFocused += DisableControls;
        CommandLineController.onCommandLineDeFocused += EnableControls;
    }

    private void OnDisable()
    {
        CommandLineController.onCommandLineFocused -= DisableControls;
        CommandLineController.onCommandLineDeFocused -= EnableControls;
    }

    public void DisableControls()
    {
        GetComponent<vThirdPersonInput>().enabled = false;
    }

    public void EnableControls()
    {
        GetComponent<vThirdPersonInput>().enabled = true;
    }
}
