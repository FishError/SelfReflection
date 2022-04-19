using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CorruptionState
{
    Corrupted,
    NotCorrupted
}

[Serializable]
class CorruptedObject
{
    public CorruptionState state;
    public GameObject gameObject;
    public Material normalMaterial;
    public Material corruptedMaterial;
}

public class CorruptionManager : MonoBehaviour
{
    [SerializeField]
    List<CorruptedObject> objects;

    private void OnValidate()
    {
        foreach (var o in objects)
        {
            if (o.state == CorruptionState.Corrupted)
            {
                o.gameObject.GetComponent<MeshRenderer>().material = o.corruptedMaterial;
            }else if (o.state == CorruptionState.NotCorrupted)
            {
                o.gameObject.GetComponent<MeshRenderer>().material = o.normalMaterial;
            }
        }
    }
}
