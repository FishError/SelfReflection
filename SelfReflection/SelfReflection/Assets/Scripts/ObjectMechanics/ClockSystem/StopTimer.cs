using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTimer : MonoBehaviour
{
    public AlarmClock canvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canvas.GetComponent<AlarmClock>().timerIsRunning = false;
        }
    }
}
