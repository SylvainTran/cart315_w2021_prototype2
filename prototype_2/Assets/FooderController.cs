using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooderController : MonoBehaviour
{
    public GameObject fodderPrefab;

    public void GetFodder()
    {
        Instantiate(fodderPrefab, transform);
    }
}
