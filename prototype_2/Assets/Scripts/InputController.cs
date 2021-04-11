using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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

    }

    public void EnableControls()
    {

    }

    public float speed = 1f;
    public Camera camera;
    public GameObject followTransform;

    private void Update()
    {

    }
}
