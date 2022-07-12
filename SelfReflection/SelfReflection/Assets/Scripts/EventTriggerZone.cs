using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggerZone : MonoBehaviour
{

    public UnityEvent myEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (myEvent != null)
        {
            myEvent.Invoke();
        }
    }
}
