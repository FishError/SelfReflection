using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public NextStageManager manager;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Ethereal") || col.CompareTag("Real"))
        {
            manager.trigger();
        }
    }
}
