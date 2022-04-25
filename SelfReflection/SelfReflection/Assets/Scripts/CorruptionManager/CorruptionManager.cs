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
class CorruptedSections
{
    public List<GameObject> objects;
    public List<LevelStage> corruptionLevels;
    public Material normalMaterial;
    public Material corruptedMaterial;
}

public class CorruptionManager : MonoBehaviour
{
    [SerializeField]
    List<CorruptedSections> sections;

    private void OnValidate()
    {
        LevelManager.onLevelChange += OnLevelChange;
    }

    private void OnDestroy()
    {
        LevelManager.onLevelChange -= OnLevelChange;
    }

    public void OnLevelChange(LevelStage levelStage)
    {
        foreach (var s in sections)
        {
            if (s.corruptionLevels.Contains(levelStage))
            {
                foreach (var o in s.objects)
                {
                    o.gameObject.GetComponent<MeshRenderer>().material = s.corruptedMaterial;
                }
            }
            else
            {
                foreach (var o in s.objects)
                {
                    o.gameObject.GetComponent<MeshRenderer>().material = s.normalMaterial;
                }
            }
        }
    }
}
