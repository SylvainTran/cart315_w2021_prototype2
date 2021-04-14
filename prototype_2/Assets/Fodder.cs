using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fodder : MonoBehaviour
{
    public int fodderCount;
    public int feedValue; // satiety on cubs

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Cub"))
        {
            if(other.gameObject.GetComponent<Cub>().Satiety < 100)
            {
                other.gameObject.GetComponent<Cub>().Satiety += feedValue;
                Destroy(this.gameObject);
            }
        }    
    }
}
