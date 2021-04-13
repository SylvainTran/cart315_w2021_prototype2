using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellMeatProduce : MonoBehaviour
{
    public Coroutine fx;
    public Coroutine killFX;

    private void Awake()
    {
        killFX = StartCoroutine(KillFX(0.0f));
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.CompareTag("meatProduce"))
        {
            // Get rewards
            Reward reward = other.gameObject.GetComponent<Reward>();
            AccountBalanceAI.UpdateMoney(reward.MoneyReward);
            fx = StartCoroutine("PlayFXThenDie", new string[]{"auraBubbleFX"});
            GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
        }    
    }

    public void PlayFXThenDie(string[] targetTags)
    {
        ParticleSystem[] childrenParticleSytems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach( ParticleSystem childPS in childrenParticleSytems )
        {
            foreach(string t in targetTags)
            {
                if(childPS.gameObject.CompareTag(t)) {
                    childPS.Play();
                }                
            }
        }
        if(killFX != null)
        {
            StopCoroutine(killFX);
        }
        killFX = StartCoroutine(KillFX(5.0f));
    }

    private IEnumerator KillFX(float delay)
    {
        yield return new WaitForSeconds(delay);
        ParticleSystem[] childrenParticleSytems = gameObject.GetComponentsInChildren< ParticleSystem >();
        foreach( ParticleSystem childPS in childrenParticleSytems )
        {
            childPS.Stop();
        }
        if(fx != null && killFX != null)
        {
            StopCoroutine(fx);
            StopCoroutine(killFX);
        }
    }
}
