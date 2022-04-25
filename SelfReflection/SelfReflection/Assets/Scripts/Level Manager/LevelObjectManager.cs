using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
class DynamicLevelObject
{
    public GameObject gameObject;
    public List<LevelStage> activeLevels;
}

public class LevelObjectManager : MonoBehaviour
{
    [SerializeField]
    private List<DynamicLevelObject> objects;

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
        foreach (var o in objects)
        {
            if (o.activeLevels.Contains(levelStage))
            {
                o.gameObject.SetActive(true);
            }
            else
            {
                o.gameObject.SetActive(false);
            }
        }
    }
}
