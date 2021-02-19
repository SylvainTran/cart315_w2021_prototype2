using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject COUNTER_DISPLAY_1;
    public GameObject COUNTER_DISPLAY_2;
    public GameObject STAT_DISPLAY_1;

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    public static void UpdateCounterDisplay()
    {
        GameObject template = GameObject.FindWithTag("template");
        if(template)
        {
            template.gameObject.GetComponent<TextMeshProUGUI>().SetText("");
        }
    }

    private IEnumerator ResetCounterDisplay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject template = GameObject.FindWithTag("template");
        template.gameObject.GetComponent<TextMeshProUGUI>().SetText("");
    }
}
