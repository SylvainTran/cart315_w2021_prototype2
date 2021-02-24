using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpdateClientsUI : MonoBehaviour
{  
    public GameObject buttonPrefab;
    public GameObject textRatingPrefab;  
    private float startingPoint = 0.0f;
    private float buttonOffset = 25.0f;  
    private Vector2 canvasWidthHeight;    
    public GameObject panel;

    private void Start()
    {
        RectTransform parentCanvas = panel.GetComponent<RectTransform>();
        canvasWidthHeight = new Vector2(parentCanvas.rect.width, parentCanvas.rect.height);
        // startingPoint = canvasWidthHeight.y;
        startingPoint = 350.0f;
    }
    private void OnEnable() {
        SceneController.onClockTicked += UpdateCubRatingsUI;    
        Main.onCharactersLoaded += InitProfileFields;
    }

    private void OnDisable() {
        SceneController.onClockTicked -= UpdateCubRatingsUI;  
        Main.onCharactersLoaded -= InitProfileFields;  
    }
    
    public void InitProfileFields()
    {
        // Access the parent's Cub component for data, then fill out the UI's fields
        foreach(Cub c in Main.currentCubRooster) 
        {
            //public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
            GameObject b = Instantiate(buttonPrefab, new Vector3(368.0f, startingPoint - buttonOffset, 0.0f), Quaternion.identity);
            b.transform.SetParent(this.gameObject.transform);
            b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText($"{c.characterName}");
            startingPoint -= buttonOffset;
            // Add event listener
            b.GetComponent<Button>().onClick.AddListener( delegate{SellCub(c, b);} );
        }
    }

    public void SellCub(Cub c, GameObject b)
    {
        Cub[] dest = new Cub[Main.currentCubRooster.Length - 1];

        for(int i = 0; i < Main.currentCubRooster.Length; i++)
        {
            if(Main.currentCubRooster[i] == c) {
                if( i > 0 )
                    Array.Copy(Main.currentCubRooster, 0, dest, 0, i);
                if( i < Main.currentCubRooster.Length - 1 )
                    Array.Copy(Main.currentCubRooster, i + 1, dest, i, Main.currentCubRooster.Length - i - 1);
            }
        }
        // Add money to player account
        AccountBalanceAI.UpdateMoney(c.valueRating);
        // Delete go and button
        Destroy(c.gameObject);
        Destroy(b.gameObject);
    }

    public void UpdateCubRatingsUI()
    {
        foreach(Cub c in Main.currentCubRooster) {
            // Generate a button to show the name and value rating for each cub in rooster
        }
        // performanceLevel.GetComponent<TextMeshProUGUI>().SetText($"Performance Level (0-10): {cubData.performanceLevel}");        
    }

    public void ShowCanvas(bool value)
    {
        GetComponent<Canvas>().enabled = value;
    }

    public void CloseButton()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
