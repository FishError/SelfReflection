using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public Queue<string> subtitles = new Queue<string>();
    public TMP_Text textBox;
    // Start is called before the first frame update
    void Start()
    {
        //Console.Write("Text Box has updated.");
        textBox = GameObject.Find("SubtitleText").GetComponent<TMP_Text>();
        Console.Write("Text Box has updated.");

    }
        // Update is called once per frame
        void Update()
    {
        
    }

    public void addSubtitle(string audioText)
    {
        subtitles.Enqueue(audioText);
    }

    void playSubtitle()
    {
        textBox.text = subtitles.Dequeue();
    }
}
