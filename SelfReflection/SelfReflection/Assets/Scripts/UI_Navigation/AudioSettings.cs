using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public FMOD.Studio.Bus Music;
    public FMOD.Studio.Bus SFX;
    public FMOD.Studio.Bus Master;
    public Slider musicSlider;
    public Slider sfxSlider = null, masterSlider = null;

    public void MasterVolumeLevel(float sliderValue)
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
        Master.setVolume(sliderValue);
        PlayerPrefs.SetFloat("MasterVol", sliderValue);
    }

    public void SFXVolumeLevel(float sliderValue)
    {
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        SFX.setVolume(sliderValue);
        PlayerPrefs.SetFloat("SfxVol", sliderValue);
    }

    public void MusicVolumeLevel(float sliderValue)
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        Music.setVolume(sliderValue);
        PlayerPrefs.SetFloat("MusicVol", sliderValue);
        
    }
}
