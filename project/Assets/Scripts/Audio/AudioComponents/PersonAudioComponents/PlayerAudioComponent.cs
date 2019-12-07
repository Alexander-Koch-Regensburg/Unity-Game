using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioComponent : PersonAudioComponent
{
    public override void Play(string name)
    {
        DynamicSound s = GetDynamicSound(name);
        s.ConfigureAudioSource(audioSource);

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        CheckIfEnemyIsInRange(s);
    }
}
