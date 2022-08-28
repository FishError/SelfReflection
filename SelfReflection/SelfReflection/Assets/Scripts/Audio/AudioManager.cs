using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject[] audioObjects;
    public GameObject currentAudioObject;
    public GameObject backgroundMusic;
    public float playTime = 0f;
    public bool startPlayback = false;
    
    // Start is called before the first frame update
    void Start()
    {
        currentAudioObject = audioObjects[0];
        backgroundMusic.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (startPlayback)
        {
            play();
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
