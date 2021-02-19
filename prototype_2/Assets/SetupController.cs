using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupController : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        print("Write your Momma Cub Club's name!");
        // Generate cub sheets, trainees with stats to help in life.
        // Submit button handler
        //cubAdminFormSubmit.onClick.AddListener(ClickedCubAdminFormSubmit);

    }

    public void InputFieldChanged()
    {
        // Raise event?
    }

    public void ClickedCubAdminFormSubmit()
    {
        print("Validating input fields of form #1");
    }
}
