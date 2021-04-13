using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSources : MonoBehaviour
{
    public AudioSource[] audioSources;

    private void OnEnable()
    {
        CommandLineController.onDeclaredBankruptcy += BankruptcyFX;
    }

    private void OnDisable()
    {
        CommandLineController.onDeclaredBankruptcy -= BankruptcyFX;
    }

    public void Awake()
    {
        audioSources = GetComponents<AudioSource>();
    }

    public void BankruptcyFX()
    {
        foreach(AudioSource a in audioSources)
        {
            a.Play();
        }
    }
}
