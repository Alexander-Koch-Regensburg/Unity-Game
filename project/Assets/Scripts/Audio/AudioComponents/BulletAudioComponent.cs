using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAudioComponent : AudioComponent
{
    public  DynamicSound onEnvironmentCollision;
    public  DynamicSound onPersonCollision;

    protected override void Awake()
    {
        AddSound(onEnvironmentCollision);
        AddSound(onPersonCollision);
        base.Awake();

    }

    public void Play2DClipAtPoint(string name, Vector2 position)
    {
        //  Create a temporary audio source object
        GameObject tempAudioSource = new GameObject("TempAudio");

        // Set Position
        tempAudioSource.transform.position = position;

        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();

        DynamicSound s = GetDynamicSound(name);
        s.ConfigureAudioSource(audioSource);
        
        CheckIfEnemyIsInRange(s);
        
        if (!audioSource.loop || !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        //  Set it to self destroy
        Destroy(tempAudioSource, s.clip.length);

    }

}
