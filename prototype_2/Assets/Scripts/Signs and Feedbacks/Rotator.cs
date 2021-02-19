using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    GameObject[] rotatingComponents;
    public Vector3 rotationAxis;
    // Start is called before the first frame update
    void Start()
    {
        rotatingComponents = GameObject.FindGameObjectsWithTag("RotatingComponent");
        rotationAxis = new Vector3(2f, 2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject rc in rotatingComponents) {
            rc.transform.Rotate(rotationAxis);
        }
    }
}
