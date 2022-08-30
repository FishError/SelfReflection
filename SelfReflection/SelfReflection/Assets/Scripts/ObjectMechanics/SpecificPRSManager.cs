using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpecificPRSManager : MonoBehaviour
{
    List<GameObject> completed;
    [Tooltip("The list of GameObject references to keep track of.")]
    [SerializeField] private List<GameObject> _allSpecifics;
    [Tooltip("Callbacks for when all object are in the zone and in correct position, rotation, and size.")]
    [SerializeField] private UnityEvent _onAllObjectInPosition;
    void Start(){
        completed = new List<GameObject>();
    }
    void Update()
    {
        CheckAllZones();
    }
    public void CheckAllZones()
    {
        foreach (var s in _allSpecifics)
        {
            if (s.GetComponent<SpecificPRSChecker>().inPosition && !completed.Contains(s))
            {
                completed.Add(s);
            }
        }
        Debug.Log(completed.Count);
        if (completed.Count == _allSpecifics.Count)
        {
            _onAllObjectInPosition?.Invoke();
        }
    }
}