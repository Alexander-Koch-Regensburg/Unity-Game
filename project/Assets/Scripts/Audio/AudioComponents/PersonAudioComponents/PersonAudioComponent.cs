using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonAudioComponent : AudioComponent
{
    public DynamicSound moving;

    protected override void Awake()
    {
        base.Awake();
        AddSound(moving);
    }

    public override void Play(string name)
    {
        SetupAudioSource(name);
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

}
