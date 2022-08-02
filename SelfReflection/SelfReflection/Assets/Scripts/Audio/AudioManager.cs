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
    public class audioSubtitlePair
    {
        public GameObject audioObject;
        public Dictionary<string, float>subtitleDictiontionary;
    }

    public class audioLengthPair
    {
        public string subtitle;
        public float length;
    }

    public List<audioSubtitlePair> audioList = new List<audioSubtitlePair>();
    Dictionary<GameObject, Dictionary<string, float>> audioObjects = new Dictionary<GameObject, Dictionary<string, float>>();
    public GameObject currentAudioObject;
    private int audioLength = 10;
    public GameObject backgroundMusic;
    public SubtitleManager subtitleManager;
    public float playTime = 0f;
    public bool startPlayback = false;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Debug.Log("start size: " + audioLength);
        foreach (var kvp in audioList)
        {
            audioObjects[kvp.audioObject] = kvp.subtitleDictiontionary;
        }
        backgroundMusic.SetActive(true);
        subtitleManager = GameObject.Find("SubtitleManager").GetComponent<SubtitleManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        getAudioLength();
        if (startPlayback)
        {
            play();
            subtitleManager.addSubtitle(audioObjects[currentAudioObject], audioLength);
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

    public void getAudioLength()
    {
        
        StudioEventEmitter tempAudio = currentAudioObject.GetComponent<StudioEventEmitter>();
        UnityEngine.Debug.Log(tempAudio);
        tempAudio.EventInstance.getDescription(out EventDescription tempAudioDescription);
        //UnityEngine.Debug.Log(tempAudioDescription.getLength(out uint length));
        //tempAudioDescription.setCallback(EVENT_CALLBACK callback, EVENT_CALLBACK_TYPE callbackmask = EVENT_CALLBACK_TYPE.ALL);
        //tempAudioDescription.setCallback(tempAudioDescription.getLength(out int tempAudioLength);
        
        tempAudioDescription.getLength(out int tempAudioLength);
        
        
        UnityEngine.Debug.Log(tempAudioDescription.isValid());
        UnityEngine.Debug.Log("old size: " + audioLength);
        audioLength = tempAudioLength;
        UnityEngine.Debug.Log("new size: " + audioLength);

    }
}
