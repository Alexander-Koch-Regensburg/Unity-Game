using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public class DynamicSound : Sound
{
    public float spatialBlend = 1.0f;
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Custom;
    public AudioSourceCurveType sourceCurveType = AudioSourceCurveType.CustomRolloff;
    public AnimationCurve animationCurve;

    public float noise = 210f;
    public int maxPlayerDistance = 15;
    public float maxEnemyDistance = 10f;


    public override void ConfigureAudioSource(AudioSource source)
    {
        base.ConfigureAudioSource(source);
        source.spatialBlend = spatialBlend;
        source.maxDistance = maxPlayerDistance;
        source.rolloffMode = rolloffMode;

        if (animationCurve == null)
        {
            animationCurve = new AnimationCurve();
        }

        if (animationCurve.length == 0)
        {
            animationCurve.AddKey(1, 1);
            animationCurve.AddKey(0, 0);
        }

        if (sourceCurveType == AudioSourceCurveType.CustomRolloff)
            source.SetCustomCurve(sourceCurveType, animationCurve);

    }
}
