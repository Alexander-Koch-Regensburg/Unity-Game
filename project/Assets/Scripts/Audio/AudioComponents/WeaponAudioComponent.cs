using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudioComponent : AudioComponent
{
    public DynamicSound onCollecting;
    public DynamicSound onNoAmmoFiring;
    public DynamicSound onFiring;

    protected override void Awake()
    {
        base.Awake();
        AddSound(onCollecting);
        AddSound(onNoAmmoFiring);
        AddSound(onFiring);
    }

    public override void Play(string name)
    {
        DynamicSound s = GetDynamicSound(name);
        if (s == null) {
            return;
        }
        s.ConfigureAudioSource(audioSource);

        if (!audioSource.loop)
        {
            audioSource.Play();
        }
        else if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        CheckIfEnemyIsInRange(s);
    }


}
