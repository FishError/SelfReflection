using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOneHubCompleteSubScript : MonoBehaviour
{
    public GameObject textBox;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TheSequence());
    }
    IEnumerator TheSequence()
    {
        textBox.GetComponent<Text>().text = "So the place seems to change each time I find my way out";
        yield return new WaitForSeconds(4);
        textBox.GetComponent<Text>().text = "the gap in the bed also appears smaller...";
        yield return new WaitForSeconds(3);
        textBox.GetComponent<Text>().text = "I guess I have no other choice but to keep going.";
        yield return new WaitForSeconds(3);
        textBox.GetComponent<Text>().text = "There appears to be something on the other side of the bed.";
        yield return new WaitForSeconds(3);
        textBox.GetComponent<Text>().text = "I know it from somewhere...I swear. I'm so close to it.";
        yield return new WaitForSeconds(5);
        textBox.GetComponent<Text>().text = "";
    }
}
