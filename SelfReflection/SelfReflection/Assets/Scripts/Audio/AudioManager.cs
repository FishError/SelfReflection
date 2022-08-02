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
    public class KeyValuePair
    {
        public GameObject audioObject;
        public string subtitleText;
    }

    public List<KeyValuePair> audioList = new List<KeyValuePair>();
    Dictionary<GameObject, string> audioObjects = new Dictionary<GameObject, string>();
    public GameObject currentAudioObject;
    public float audioLength = 0f;
    public GameObject backgroundMusic;
    public SubtitleManager subtitleManager;
    public float playTime = 0f;
    public bool startPlayback = false;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var kvp in audioList)
        {
            audioObjects[kvp.audioObject] = kvp.subtitleText;
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
        UnityEngine.Debug.Log(tempAudioDescription.getLength(out int length));
        tempAudioDescription.getLength(out int tempAudioLength);
        UnityEngine.Debug.Log(tempAudioLength);
        audioLength = tempAudioLength;
        
        
    }
}
