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

    void Start(){
        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
    }

    public void MasterVolumeLevel(float sliderValue)
    {
        Master.setVolume(sliderValue);
        PlayerPrefs.SetFloat("MasterVol", sliderValue);
    }

    public void SFXVolumeLevel(float sliderValue)
    {
        SFX.setVolume(sliderValue);
        PlayerPrefs.SetFloat("SfxVol", sliderValue);
    }

    public void MusicVolumeLevel(float sliderValue)
    {
        Music.setVolume(sliderValue);
        PlayerPrefs.SetFloat("MusicVol", sliderValue);
        
    }
}
