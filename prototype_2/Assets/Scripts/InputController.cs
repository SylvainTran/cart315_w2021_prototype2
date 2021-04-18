using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputController : MonoBehaviour
{
    public GameObject player;
    // Cams
    public static GameObject globalCam;

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

    private void Start()
    {
        globalCam = GameObject.FindGameObjectWithTag("GlobalCam");
    }

    private void FixedUpdate()
    {
        if(Terminal.inTerminalRange && Input.anyKeyDown)
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) return;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            if(!CommandLineController.commandLine.isFocused) {
                Terminal.currentTerminal.GetComponent<Terminal>().UpdateActiveCamera(this.gameObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMapCameraView();
        }
    }

    public void ReturnToMapCameraView()
    {
        CinemachineVirtualCamera[] cams = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
        for (int i = 0; i < cams.Length; i++)
        {
            CinemachineVirtualCamera c = cams[i].GetComponent<CinemachineVirtualCamera>();
            if (c)
            {
                c.Priority = 0;
            }
        }
        globalCam.GetComponent<CinemachineVirtualCamera>().Priority = 200;
        Main.playerState = (int)Main.PLAYER_STATES.MAP;
    }
}