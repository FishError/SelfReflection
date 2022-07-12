using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOneHubSemiCompleteSubScript : MonoBehaviour
{
    public GameObject textBox;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TheSequence());
    }
    IEnumerator TheSequence()
    {
        textBox.GetComponent<Text>().text = "I'm....back in the Bedroom? Why?...am I missing something?";
        yield return new WaitForSeconds(5);
        textBox.GetComponent<Text>().text = "Things do seem a little less fragmented though.";
        yield return new WaitForSeconds(5);
        textBox.GetComponent<Text>().text = "";
    }
}
