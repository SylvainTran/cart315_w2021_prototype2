using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpdateProgramManagementUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject textRatingPrefab;  
    private float startingPoint = 0.0f;
    private float buttonOffset = 25.0f;  
    private Vector2 canvasWidthHeight;    
    public GameObject panel;
    public Cub[] cubsInShop;

    private void Start()
    {
        RectTransform parentCanvas = panel.GetComponent<RectTransform>();
        canvasWidthHeight = new Vector2(parentCanvas.rect.width, parentCanvas.rect.height);
        // startingPoint = canvasWidthHeight.y;
        startingPoint = Camera.main.pixelHeight;
    }
    private void OnEnable() {
        Main.onCharactersLoaded += InitProfileFields;
    }

    private void OnDisable() {
        Main.onCharactersLoaded -= InitProfileFields;  
    }
    
    public void InitProfileFields()
    {
        cubsInShop = new Cub[Main.MAX_CUB_CAPACITY];
        for(int i = 0; i < Main.MAX_CUB_CAPACITY; i++)
        {
            cubsInShop[i] = Main.CharacterFactory.GenerateNewCub();
            cubsInShop[i].GenerateStats();
        }
        foreach(Cub c in cubsInShop) 
        {
            //public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
            GameObject b = Instantiate(buttonPrefab, new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight - 50 + startingPoint - buttonOffset, 0.0f), Quaternion.identity);
            b.transform.SetParent(this.gameObject.transform);
            b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText($"{c.characterName}");
            startingPoint -= buttonOffset;
            // Add event listener
            b.GetComponent<Button>().onClick.AddListener( delegate{BuyCub(c, b);} );
        }
    }

    public void BuyCub(Cub c, GameObject b)
    {
        if(Main.currentCubRooster.Length >= Main.MAX_CUB_CAPACITY) {
            print("Rooster full, cannot buy cub.");
            return;
        }
        Cub[] dest = new Cub[cubsInShop.Length - 1];

        for(int i = 0; i < cubsInShop.Length; i++)
        {
            if(cubsInShop[i] == c) {
                if( i > 0 )
                    Array.Copy(cubsInShop, 0, dest, 0, i);
                if( i < cubsInShop.Length - 1 )
                    Array.Copy(cubsInShop, i + 1, dest, i, cubsInShop.Length - i - 1);
            }
        }
        // Remove money to player account
        AccountBalanceAI.UpdateMoney(-c.valueRating);
        // Delete button and append go to current cub rooster
        if(Main.currentCubRooster.Length == 0) {
            Main.currentCubRooster = new Cub[Main.MAX_CUB_CAPACITY];
        }
        Main.currentCubRooster[Main.currentCubRooster.Length - 1] = c;        
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
