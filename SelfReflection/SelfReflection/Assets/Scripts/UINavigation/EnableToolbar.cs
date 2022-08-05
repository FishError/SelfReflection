using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableToolbar : MonoBehaviour
{
    private GameObject toolbar;
    private CameraPanningController cameraPanning = null;

    private void Start()
    {
        cameraPanning = GameObject.Find("CameraPanningController").GetComponent<CameraPanningController>();
        toolbar = GameObject.Find("Toolbar").gameObject;
    }

    private void Update()
    {
        if(cameraPanning != null)
        {
            if (cameraPanning.isPanning)
            {
                toolbar.SetActive(false);
            }
            else
            {
                toolbar.SetActive(true);
            }   
        }
    }
}
