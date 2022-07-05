using System;
using UnityEngine;

public class SceneVisitController : MonoBehaviour
{
    [SerializeField] private SceneIndex _currentScene;

    private void Start() => SaveSystem.SaveLevel(_currentScene);
}