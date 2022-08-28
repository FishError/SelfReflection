using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hub2SceneSwitching : MonoBehaviour
{
    public string nextScene;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(sceneName: nextScene);
    }
}
