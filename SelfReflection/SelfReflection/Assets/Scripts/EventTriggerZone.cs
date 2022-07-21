using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggerZone : MonoBehaviour
{

    public UnityEvent myEvent;

    [Tooltip("If parent object does not contain colliders, add the child object containing the collider")]
    public List<GameObject> objectsAbleToTriggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (myEvent != null && objectsAbleToTriggerEvent.Contains(other.gameObject))
        {
            myEvent.Invoke();
        }
    }
}
