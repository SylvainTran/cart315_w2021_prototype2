using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CustomButtonHandler : MonoBehaviour, IPointerClickHandler
{
    public GameObject MenuRootParent;
    private PanelOffsetToggler PanelOffsetToggler;
    private MenuDataWriter MenuDataWriter;
    public GameObject buttonBGPanel;

    private void OnEnable()
    {
        CommandLineController.onWorkerStatisticsUpdated += RefreshListingData;
        TaskController.onTaskStatisticsUpdated += RefreshListingData;
    }

    private void OnDisable()
    {
        CommandLineController.onWorkerStatisticsUpdated -= RefreshListingData;
        TaskController.onTaskStatisticsUpdated -= RefreshListingData;
    }

    private void Awake()
    {
        PanelOffsetToggler = MenuRootParent.GetComponent<PanelOffsetToggler>();
        MenuDataWriter = MenuRootParent.GetComponent<MenuDataWriter>();
        buttonBGPanel.GetComponent<Image>().enabled = false;
    }

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user right-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.

        }

        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            PanelOffsetToggler.menuButtonWasToggled = !PanelOffsetToggler.menuButtonWasToggled;
            PanelOffsetToggler.ToggleButtonInMenu(this.gameObject);
            MenuDataWriter.SetFormattedDataLine(buttonBGPanel);
            buttonBGPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;                    
            buttonBGPanel.GetComponent<Image>().enabled = !buttonBGPanel.GetComponent<Image>().enabled;
        }
        if(buttonBGPanel.GetComponent<Image>().enabled == false)
        {
            buttonBGPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
            buttonBGPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;                    
        }
    }

    public void RefreshListingData()
    {
        MenuDataWriter.SetFormattedDataLine(buttonBGPanel);        
    }
}
