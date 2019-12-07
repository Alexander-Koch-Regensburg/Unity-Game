using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MixVolumes: MonoBehaviour
{
    public AudioMixer masterMixer;

    public void SetMasterVolume(float sliderValue)
    {
        masterMixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVolume(float sliderValue)
    {
        masterMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetFfxVolume(float sliderValue)
    {
        masterMixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetWeaponVolume(float sliderValue)
    {
        masterMixer.SetFloat("WeaponVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetEnvironmentVolume(float sliderValue)
    {
        masterMixer.SetFloat("EnvironmentVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetFootstepsVolume(float sliderValue)
    {
        masterMixer.SetFloat("FootstepsVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetPlayerDeathVolume(float sliderValue)
    {
        masterMixer.SetFloat("PlayerDeathVol", Mathf.Log10(sliderValue) * 20);
    }

}
