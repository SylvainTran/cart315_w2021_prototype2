using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public GameObject player;
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
        player.GetComponent<GenericBehaviour>().enabled = false;
        player.GetComponent<MoveBehaviour>().enabled = false;
    }

    public void EnableControls()
    {
        player.GetComponent<GenericBehaviour>().enabled = true;
        player.GetComponent<MoveBehaviour>().enabled = true;
    }
}
