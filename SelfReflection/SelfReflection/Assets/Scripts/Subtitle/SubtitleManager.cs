using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public Queue<string> subtitles = new Queue<string>();
    public TMP_Text textBox;
    public float audioLength;
    // Start is called before the first frame update
    void Start()
    {
        //Console.Write("Text Box has updated.");
        textBox = GameObject.Find("SubtitleText").GetComponent<TMP_Text>();
        
    }
        // Update is called once per frame
    void Update()
    {

        if (subtitles != null) {
            playSubtitle();
            textBox.enabled = true;
            Invoke("closeSubtitle", audioLength);
        }
        
        
    }

    public void addSubtitle(string audioText , float length)
    {
        subtitles.Enqueue(audioText);
        audioLength = length;
    }

    void playSubtitle()
    {
        textBox.text = subtitles.Dequeue();
        
    }

    void closeSubtitle()
    {
        textBox.enabled = false;
    } 
}
