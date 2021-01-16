using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudio : MonoBehaviour
{

    [SerializeField] private AudioClip[] clips;
    [SerializeField] float minPitch, maxPitch;

    private AudioSource source;
    private bool playOnAwake;

    public void Play()
    {
        source.pitch = minPitch + Random.value * (maxPitch - minPitch);
        if (clips != null && clips.Length > 0)
        {
            source.clip = clips.Random1();
        }
        source.Play();
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        playOnAwake = source.playOnAwake;
        source.playOnAwake = false;
        source.Stop();
    }

    private void OnEnable()
    {
        if (playOnAwake)
        {
            Play();
        }
    }

}