using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public abstract class Sound
{
    public string name;
    public AudioMixerGroup mixerGroup;
    public bool loop = false;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1;

    public AudioClip clip;

    public virtual void ConfigureAudioSource(AudioSource source)
    {
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.outputAudioMixerGroup = mixerGroup;
    }
}
