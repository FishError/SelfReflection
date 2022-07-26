using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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
        subtitleManager = GetComponent<SubtitleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startPlayback)
        {
            play();
            Debug.Log(audioObjects[currentAudioObject]);
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
