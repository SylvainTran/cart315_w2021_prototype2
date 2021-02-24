using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateCubProfileUI : MonoBehaviour
{
    public GameObject characterName;
    public GameObject characterVariant;
    public GameObject performanceLevel;   
    Cub cubData;

    private void OnEnable() {
        Main.onCharactersLoaded += InitProfileFields;    
    }

    private void OnDisable() {
        Main.onCharactersLoaded -= InitProfileFields;        
    }
    
    public void InitProfileFields()
    {
        // Access the parent's Cub component for data, then fill out the UI's fields
        cubData = transform.parent.GetComponent<Cub>();
        characterName.GetComponent<TextMeshProUGUI>().SetText(cubData.characterName);
        characterVariant.GetComponent<TextMeshProUGUI>().SetText(cubData.characterVariant);        
        performanceLevel.GetComponent<TextMeshProUGUI>().SetText($"Performance Level (0-10): {cubData.performanceLevel}");        
    }

    public void UpdatePerformanceLevelUI()
    {
        performanceLevel.GetComponent<TextMeshProUGUI>().SetText($"Performance Level (0-10): {cubData.performanceLevel}");        
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
