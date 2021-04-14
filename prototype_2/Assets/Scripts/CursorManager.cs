using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D interactibleCursorTexture;
    public Texture2D errorCursorTexture;
    public Texture2D menuCursorTexture;    
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public void Start()
    {
        SetDefaultCursor();
    }

    public void SetInteractibleCursor()
    {
        Cursor.SetCursor(interactibleCursorTexture, hotSpot, cursorMode);
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void SetErrorCursor()
    {
        Cursor.SetCursor(errorCursorTexture, hotSpot, cursorMode);
    }

    public void SetMenuCursor()
    {
        Cursor.SetCursor(menuCursorTexture, hotSpot, cursorMode);
    }
}
