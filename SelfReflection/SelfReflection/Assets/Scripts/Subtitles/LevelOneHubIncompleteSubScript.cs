using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOneHubIncompleteSubScript : MonoBehaviour
{
    public GameObject textBox;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TheSequence());
    }
    IEnumerator TheSequence()
    {
        textBox.GetComponent<Text>().text = "I'm not sure why everything's so fragmented, but this looks like a Bedroom.";
        yield return new WaitForSeconds(6);
        textBox.GetComponent<Text>().text = "Was this my Bedroom? I'm not sure, nothing looks familiar...";
        yield return new WaitForSeconds(7);
        textBox.GetComponent<Text>().text = "";
    }
}
