using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelStage
{
    Level1,
    Level2,
    Level3,
}


public class LevelManager : MonoBehaviour
{
    public delegate void OnLevelChange(LevelStage levelStage);
    public static event OnLevelChange onLevelChange;

    [SerializeField]
    private LevelStage currentStage;

    private void OnValidate()
    {
        onLevelChange(currentStage);
    }
}
