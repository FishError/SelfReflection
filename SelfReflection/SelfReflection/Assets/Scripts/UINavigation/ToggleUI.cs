using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject canvas;
    public GameObject player;
    public Collider body;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveUI()
    {
        canvas.SetActive(true);
    }

    public void SetUnactiveUI()
    {
        canvas.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) {
            SetActiveUI();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        SetUnactiveUI();
    }
}
