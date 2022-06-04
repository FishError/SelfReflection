using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAudio : MonoBehaviour
{
    public GameObject audioObject;
    public AudioManager audioManager;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
       
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == player.tag)
        {
            audioManager.setCurAudioObject(audioObject);
            audioManager.setPlayback(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        audioManager.setCurAudioObject(audioObject);
        audioManager.setPlayback(true);
    
    }
}
