using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildAtPoint : MonoBehaviour
{
    public GameObject prefab;
    public float delay = 15.0f;
    // Update is called once per frame
    void FixedUpdate()
    {
       if(Input.GetMouseButton(0)) {
            ++delay;
            if(delay >= 15)
            {
                Ray clickRay = Camera.main.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit result;
                if(Physics.Raycast(clickRay, out result))
                {
                    if (result.collider.gameObject.CompareTag("Ground"))
                    {
                        GameObject.Instantiate(prefab, result.point, Quaternion.identity);
                    }
                }
                delay = 0;
            }
       } else {
            delay = 15.0f;
       }
    }
}
