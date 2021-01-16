using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyReader : Interactible
{
    [SerializeField] int key = 1;
    [SerializeField] UnityEvent action;
    [SerializeField] AudioClip correct, wrong;
    AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }
    public override void Interact()
    {
        if (true)//Player.Instance.key[key])
        {
            audioSource.clip = correct;
            audioSource.Play();
            action.Invoke();
        }
        else
        {
            audioSource.clip = wrong;
            audioSource.Play();
        }
    }
}