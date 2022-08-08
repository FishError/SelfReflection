using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public Queue<AudioManager.AudioLengthPair> subtitles = new Queue<AudioManager.AudioLengthPair>();
    public TMP_Text textBox;
    public float audioLength;
    public AudioManager.AudioLengthPair curAudioLengthPair;
    public bool subIncomplete = true;
    // Start is called before the first frame update
    void Start()
    {
        //Console.Write("Text Box has updated.");
        textBox = GameObject.Find("SubtitleText").GetComponent<TMP_Text>();
        
        
    }
        // Update is called once per frame
    void FixedUpdate()
    {
        if (subIncomplete)
        {
            if (subtitles.Count != 0)
            {
                Debug.Log(subtitles.Count);
                StartCoroutine(playSubtitle());
                subIncomplete = false;
            }
        }
        
    }

    public void addSubtitle(List<AudioManager.AudioLengthPair> list)
    {
        foreach (var item in list)
        {
            subtitles.Enqueue(item);
        }
    }

    IEnumerator playSubtitle()
    {
        textBox.enabled = true;
        curAudioLengthPair = subtitles.Dequeue();
        textBox.text = curAudioLengthPair.subtitle;
        audioLength = curAudioLengthPair.length;
        yield return new WaitForSeconds(audioLength);
        closeSubtitle();
        subIncomplete = true;

    }

    void closeSubtitle()
    {
        textBox.enabled = false;
    } 

}
