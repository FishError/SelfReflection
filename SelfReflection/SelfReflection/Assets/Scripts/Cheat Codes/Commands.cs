using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Commands : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(sceneBuildIndex: 2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene(sceneBuildIndex: 3);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public void loadSceneOne()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }

    public void loadSceneTwo()
    {
        SceneManager.LoadScene(sceneBuildIndex: 2);
    }

    public void loadSceneThree()
    {
        SceneManager.LoadScene(sceneBuildIndex: 3);
    }

    public void loadSceneFour()
    {
        SceneManager.LoadScene(sceneBuildIndex: 4);
    }
}
