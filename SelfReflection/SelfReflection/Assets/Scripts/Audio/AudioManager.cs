using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;


public class AudioManager : MonoBehaviour
{
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

    public Queue<GameObject> audioQueue = new Queue<GameObject>();
    public GameObject currentAudioObject;
    //private int curAudioLength = 10;
    public GameObject backgroundMusic;
    public SubtitleManager subtitleManager;
    public bool startPlayback = false;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic.SetActive(true);
        subtitleManager = GameObject.Find("SubtitleManager").GetComponent<SubtitleManager>();
        audioQueue.Enqueue(currentAudioObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (audioQueue.Count > 0)
        {
            currentAudioObject = audioQueue.Dequeue();
            if (startPlayback)
            {
                play();
                subtitleManager.addSubtitle(audioList.Find(x => x.audioObject == currentAudioObject).audioLengthPairs);
                release();
            }
            setPlayback(false);
        }

    }

    public void setCurAudioObject(GameObject audio)
    {
        audioQueue.Enqueue(audio);
        setPlayback(true);
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
