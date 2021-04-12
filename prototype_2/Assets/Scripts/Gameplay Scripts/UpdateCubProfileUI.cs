using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateCubProfileUI : MonoBehaviour
{
    public GameObject characterName;
    public GameObject characterVariant;
    public GameObject leanness;
    public GameObject satiety;

    Cub cubData;

    private void OnEnable() {
        Main.onCharactersLoaded += InitProfileFields;
        SceneController.onClockTicked += UpdateLeannessUI;   
    }

    private void OnDisable() {
        Main.onCharactersLoaded -= InitProfileFields;  
        SceneController.onClockTicked -= UpdateLeannessUI;      
    }
    
    public void InitProfileFields()
    {
        // Access the parent's Cub component for data, then fill out the UI's fields
        cubData = transform.parent.GetComponent<Cub>();
        print(cubData);
        characterName.GetComponent<TextMeshProUGUI>().SetText(cubData.characterName);
        characterVariant.GetComponent<TextMeshProUGUI>().SetText(cubData.characterVariant);        
        leanness.GetComponent<TextMeshProUGUI>().SetText($"Leanness: {cubData.leanness}");
        satiety.GetComponent<TextMeshProUGUI>().SetText($"Hunger: {1 - cubData.Satiety}");
        print("Name test: " + cubData.characterName);
    }
    /**
     * Update performance level and hunger on clock tick.
     */
    public void UpdateLeannessUI()
    {
        if(!cubData || !leanness || !satiety) 
        {
            return;
        }
        leanness.GetComponent<TextMeshProUGUI>().SetText($"Leanness: {cubData.leanness}");
        satiety.GetComponent<TextMeshProUGUI>().SetText($"Hunger: {1 - cubData.Satiety}");
    }

    public void ShowCanvas()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void CloseButton()
    {
        GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {
        
    }
}
