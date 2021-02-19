using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public GameObject player;
    public Vector3 collisionForce;
    public bool switch0 = false;
    public bool switch1 = false;
    public bool switch2 = false;
    public float contactThreshold = 0.990f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(SceneController.StartChangeScene(0.5f));
    }

    private IEnumerator StartDialogueNode(string dialogue)
    {
        yield return new WaitForSeconds(0.5f);
        //.GetComponent<TextMesh>().text = dialogue;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // Do something
        }
    }
}
