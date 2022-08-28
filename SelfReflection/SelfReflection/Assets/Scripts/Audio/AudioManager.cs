using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public GameObject[] audioObjects;
    [Serializable]
    public class AudioSubtitlePair
    {
        public GameObject audioObject;
        public List<AudioLengthPair> audioLengthPairs;
    }
    [Serializable]
    public class AudioLengthPair
    {
        public string subtitle;
        public float length;
    }

    //Create a list of both objects and a dictionary to hold both of them
    public List<AudioSubtitlePair> audioList = new List<AudioSubtitlePair>();
    public List<AudioLengthPair> audioLengthPairs = new List<AudioLengthPair>();

    public GameObject currentAudioObject;
    //private int curAudioLength = 10;
    public GameObject backgroundMusic;
    public float playTime = 0f;
    public SubtitleManager subtitleManager;
    public bool startPlayback = false;

    // Start is called before the first frame update
    void Start()
    {
        currentAudioObject = audioObjects[0];
        backgroundMusic.SetActive(true);
        subtitleManager = GameObject.Find("SubtitleManager").GetComponent<SubtitleManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (startPlayback)
        {
            play();
            subtitleManager.addSubtitle(audioList.Find(x => x.audioObject == currentAudioObject).audioLengthPairs);
            release();
        }
        setPlayback(false);
    }

    public void setCurAudioObject(GameObject audio)
    {
        currentAudioObject = audio;
    }
    private void play()
    {
        currentAudioObject.SetActive(true);
        
    }

    private void release()
    {
        currentAudioObject.SetActive(false);
    }

    public void setPlayback(bool a)
    {
        startPlayback = a;
    }
}
