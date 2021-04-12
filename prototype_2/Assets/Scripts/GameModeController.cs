using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeController : MonoBehaviour
{
    public enum gameModes { AdventureMode, ManagementMode };
    public static int currentGameMode = 0;

    private void Start()
    {
        currentGameMode = (int)gameModes.AdventureMode;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        RegisterInput();
    }

    public void RegisterInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Switch to the other view
            if (currentGameMode == (int)gameModes.AdventureMode)
            {
                currentGameMode = (int)gameModes.ManagementMode;
            }
            else
            {
                currentGameMode = (int)gameModes.AdventureMode;
            }
            SwitchGameMode();
        }
    }

    public void SwitchGameMode()
    {
        Camera cam = GetComponent<Camera>();
        switch (currentGameMode)
        {
            case (int)gameModes.AdventureMode:
                cam.enabled = true;
                break;
            case (int)gameModes.ManagementMode:
                cam.enabled = false; // TODO kill runSpeed as well
                break;
            default: break;
        }
    }
}
