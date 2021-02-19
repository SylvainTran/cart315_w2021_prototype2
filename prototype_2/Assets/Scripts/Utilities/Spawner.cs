using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform locationBot;
    public Transform locationTop;
    public GameObject player;
    public int verticalOffsetBot = 1;
    public int verticalOffsetTop = 1;
    public float delay = 120.0f;

    // spawn prefab if player is in front of tower
    private void SpawnFromPlayerDir()
    {
        Vector3 forwardDir = transform.TransformDirection(Vector3.right);
        Vector3 dirToPlayer = transform.position - player.transform.position;
        if(Vector3.Dot(forwardDir, dirToPlayer) < 0.0f)
        {
            Instantiate(prefab, locationBot.position + new Vector3(0f, verticalOffsetBot, 0f), Quaternion.identity);
            ++verticalOffsetBot;
        } else
        {
            Instantiate(prefab, locationTop.position + new Vector3(0f, -verticalOffsetTop, 0f), Quaternion.identity);
            ++verticalOffsetTop;
        }
    }

    private void Update()
    {
        if(Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
        {
            if(delay > 120.0f)
            {
                SpawnFromPlayerDir();
                delay = 0.0f;
            }
            ++delay;
        }
    }
}
