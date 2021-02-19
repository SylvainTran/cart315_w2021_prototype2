using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemShopController : MonoBehaviour
{
    // Dependent GOs - TO REFACTOR with events
    public GameObject shopUI;
    public GameObject cameraGO;
    public GameObject UIGameCanvas;
    // Events for the player BuyTraps() component
    public delegate void BuyItemTemplate1();
    public static event BuyItemTemplate1 buyItemTemplate1;

    private void OnTriggerEnter(Collider collider)
    {
        if(!collider.gameObject.CompareTag("Player"))
        {
            return;
        }
        if(!shopUI.activeSelf)
        {
            shopUI.SetActive(true);
            UIGameCanvas.SetActive(false);
            // TODO Disable camera rotating with mouse
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (shopUI.activeSelf)
        {
            shopUI.SetActive(false);
            UIGameCanvas.SetActive(true);
            GameObject tcd = GameObject.FindWithTag("COUNTER_DISPLAY_1");
            if(tcd) {
                tcd.gameObject.GetComponent<TextMeshProUGUI>().SetText("COUNTER_DISPLAY_1: " + ItemInventory.itemsInInventoryCount);
            }
        }
    }

    public void UIBuyItemTemplate1()
    {
        buyItemTemplate1();
    }
}
