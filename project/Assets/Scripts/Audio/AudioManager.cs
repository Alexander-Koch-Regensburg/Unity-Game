using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<StaticSound> staticSounds;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (StaticSound s in staticSounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.ConfigureAudioSource(s.audioSource);
        }

    }

    void Start()
    {
        Play("Music");
    }

    public StaticSound GetStaticSound(String name)
    {
        foreach (StaticSound s in staticSounds)
        {
            if (s.name == name)
                return s;
        }
        return null;
    }

    private void Play(string name)
    {
        foreach (StaticSound s in staticSounds)
        {
            if (s.name == name)
            {
                s.audioSource.Play();
            }
        }
    }
}
