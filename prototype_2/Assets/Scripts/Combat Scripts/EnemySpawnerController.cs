using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public GameObject[] enemies;

    void Start()
    {
        GameObject[] spots = GameObject.FindGameObjectsWithTag("Spot");
        for(int i = 0; i < enemies.Length; i++)
        {
            Instantiate(enemies[i]);
            enemies[i].transform.position = spots[i].transform.position;
        }
    }
}
