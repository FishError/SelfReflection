using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipProgressBarAnimation : MonoBehaviour
{
    private float amt = 0.7f;
    public GameObject curProgress;
    public bool isPlaying = false;

    public void EnableBarAnimation()
    {
        if(curProgress.transform.localScale.x < 1)
        {
            curProgress.transform.localScale += new Vector3(amt * Time.deltaTime, 0, 0);
            isPlaying = true;
        }
        else
        {
            isPlaying = false;
        }
    }

    public void ResetBarAnimation()
    {
        curProgress.transform.localScale = new Vector3(0, 1, 1);
        isPlaying = false;
    }
}
