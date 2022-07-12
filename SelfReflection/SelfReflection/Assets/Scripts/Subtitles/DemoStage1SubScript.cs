using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DemoStage1SubScript : MonoBehaviour
{
    public GameObject textBox;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TheSequence());
    }
    IEnumerator TheSequence()
    {
        textBox.GetComponent<Text>().text = "where am I?";
        yield return new WaitForSeconds(2);
        textBox.GetComponent<Text>().text = "";
    }
}
