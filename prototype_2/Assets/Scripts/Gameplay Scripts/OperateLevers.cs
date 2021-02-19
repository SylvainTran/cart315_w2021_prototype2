using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperateLevers : MonoBehaviour
{
    private void OnTriggerStay(Collider collider)
    {
        if(collider.CompareTag("OpenAllDoorsLever"))
        {
            Debug.Log("Lever in proximity");
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                Debug.Log("Open All Doors Lever operated");
                GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                foreach(GameObject monster in monsters){
                    monster.GetComponent<MonsterAI>().SetAllAreaMask();
                }
            }
        } else if(collider.CompareTag("CloseAllDoorsLever"))
        {
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                Debug.Log("Close All Doors Lever operated");
                GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                foreach(GameObject monster in monsters){
                    monster.GetComponent<MonsterAI>().CloseCorridors();
                }
            }
        }
    }
}
