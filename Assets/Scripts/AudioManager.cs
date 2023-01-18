using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private void Awake()
    {
        foreach(Sound sound in sounds)
        {
            sound.audioSource=gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume=sound.volume;
            sound.audioSource.loop=sound.loop;
        }
    }
    private void Start()
    {
        Play("Theme");
    }
    /// <summary>
    /// Plays sounds by name
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;
        }
        if (!s.audioSource.isPlaying)
            s.audioSource.Play();
    }

}
