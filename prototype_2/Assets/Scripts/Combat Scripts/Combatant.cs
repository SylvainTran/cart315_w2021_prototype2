using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Combatant : MonoBehaviour
{
    private float health = 100.0f;
    private float exp = 10.0f;
    private int strength = 30;
    public GameObject gameOverScreen;

    public float Health {
        get { return health; }
        set {
            health += value; // positive or negative values
        }
    }

    public float Exp { // Exp returned by monster
        get { return exp; }
        set { if(value >= 0)
            {
                exp = value;
            }
        }
    }

    public int Strength {
        get { return strength; }
        set {
            if(value >= 0) {
                strength = value;
            }
        }
    }

    private bool alive = true;
    public bool Alive {
        get { return alive; }
        set {
            alive = value;
        }
    }

    public bool TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Damage taken: " + damage);
        Debug.Log("HP: " + health);
        if(health <= 0)
        {
            alive = false;
            Die();
            // Show overkill damage?
            // Trigger FX?
        }
        return alive;
    }

    private void Attack(float damage)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(!alive) {
            return;
        }
        if(this.gameObject.tag == "Monster" && other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<Combatant>().Health <= 0f) {
  
            } else {
                other.gameObject.GetComponent<Combatant>().TakeDamage(strength);
            }
        }
    }

    public void Die()
    {
        if(this.gameObject.tag == "Player")
        {
            gameOverScreen = GameObject.FindGameObjectWithTag("GameOverScreen");
            if(gameOverScreen)
            {
                gameOverScreen.SetActive(true);
            }
            return;
        }
        Debug.Log("Death ensued.");
        alive = false;
        Destroy(this.gameObject);
        // Update level temporary check
    }
}
