using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpecificPRSManager : MonoBehaviour
{
    [Tooltip("The list of GameObject references to keep track of.")]
    [SerializeField] private List<GameObject> _allSpecifics;
    [Tooltip("Callbacks for when all object are in the zone and in correct position, rotation, and size.")]
    [SerializeField] private UnityEvent _onAllObjectInPosition;

    public void CheckAllZones()
    {
        List<GameObject> completed = new List<GameObject>();
        foreach (var s in _allSpecifics)
        {
            if (s.GetComponent<SpecificPRSChecker>().inPosition)
            {
                completed.Add(s);
            }
        }

        if (completed.Count == _allSpecifics.Count)
        {
            _onAllObjectInPosition?.Invoke();
        }
    }
}