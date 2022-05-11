using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadAudio : MonoBehaviour
{
    public FMOD.Studio.Bus Music;
    public FMOD.Studio.Bus SFX;
    public FMOD.Studio.Bus Master;
    public Slider musicSlider;
    private void Start()
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
        
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            Music.setVolume(PlayerPrefs.GetFloat("MusicVol", 0.3f));
            musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 0.3f);
        }
    }

}
