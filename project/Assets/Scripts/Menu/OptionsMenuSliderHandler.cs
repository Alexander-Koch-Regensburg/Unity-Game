using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OptionsMenuSliderHandler : MonoBehaviour
{
    public Slider mainVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public MixVolumes mixVolumes;


    void Start()
    {
        mainVolumeSlider.value = PlayerPrefs.GetFloat("MainVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }
    

    public void onMainVolumeSliderChange(float sliderValue)
    {
        mixVolumes.SetMasterVolume(sliderValue);
        PlayerPrefs.SetFloat("MainVolume", mainVolumeSlider.value);
    }

    public void onMusicVolumeSliderChange(float sliderValue)
    {
        mixVolumes.SetMusicVolume(sliderValue);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
    }

    public void onSfxVolumeSliderChange(float sliderValue)
    {
        mixVolumes.SetFfxVolume(sliderValue);
        PlayerPrefs.SetFloat("SfxVolume", sfxVolumeSlider.value);
    }
}
