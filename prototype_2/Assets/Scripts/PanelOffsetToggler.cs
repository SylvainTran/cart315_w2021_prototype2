using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOffsetToggler : MonoBehaviour
{
    public List<GameObject> buttonListOrder;
    public Vector3 offsetVector;
    public bool menuButtonWasToggled = false;

    private void Awake() 
    {
        buttonListOrder = new List<GameObject>();
        int buttonsInList = transform.childCount;
        offsetVector = new Vector3(0.0f, 275.0f, 0.0f);
        print($"Nb of bts {buttonsInList}");
        for (int i = 0; i < buttonsInList; i++)
        {
            print($"Button names: {transform.GetChild(i).gameObject.name}");
            buttonListOrder.Add(transform.GetChild(i).gameObject);
        }
    }

    public void ToggleButtonInMenu(GameObject buttonCaller)
    {
        OffsetMenuButtons(buttonCaller);
    }

    // When toggling, we just create an offset with that order in mind
    public void OffsetMenuButtons(GameObject buttonCaller)
    {
        // Find all buttons after this one in the Menu order and offset them
        //List<GameObject> buttonsToOffset = new List<GameObject>();
        int index = buttonListOrder.IndexOf(buttonCaller);
        int max = buttonListOrder.Count;
        if(menuButtonWasToggled)
        {
            for(int i = index + 1; i < max; i++)
            {
                buttonListOrder[i].transform.position -= offsetVector;
            }     
        }
        else
        {
            for(int i = index + 1; i < max; i++)
            {
                buttonListOrder[i].transform.position += offsetVector;
            }     
        }
    }

    public void CalculateOffsetRequired()
    {
        // The total offset for any button is the sum of all text lines needed * height of each line * fontSize

    }
}
