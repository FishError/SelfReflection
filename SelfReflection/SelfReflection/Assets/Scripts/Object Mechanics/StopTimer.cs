using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTimer : MonoBehaviour
{
    public AlarmClock canvas;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            canvas.GetComponent<AlarmClock>().timerIsRunning = false;
        }
    }
}
