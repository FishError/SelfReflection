using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public class TutorialScene {
    public string sceneName;
    public bool completed;
    public string nextScene;
}

public class TutorialManager : MonoBehaviour
{
    public List<TutorialScene> Tutorials;
    public string currentScene;
    public string lastScene;
    public TutorialScene lastTutorialScene;
    public bool lastTutorialFirstCompletion;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        List<TutorialScene> tutorial = Tutorials.Where(t => t.sceneName == currentScene).ToList();
        if (tutorial.Any())
        {
            if (!tutorial[0].completed)
            {
                lastTutorialFirstCompletion = true;
            }
            tutorial[0].completed = true;
            lastTutorialScene = tutorial[0];
        }

        if (!Tutorials.Any(t => t.sceneName == currentScene) || lastScene == null)
        {
            lastScene = currentScene;
        }
        currentScene = next.name;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Tutorial Hub")
        {
            GameObject exit = GameObject.Find("Exit portal");
            if (exit != null)
            {
                SceneChanger sceneChanger = exit.GetComponentInChildren<SceneChanger>();
                if (lastTutorialScene.completed && !lastTutorialFirstCompletion)
                {
                    sceneChanger.nextScene = lastScene;
                }
                else
                {
                    if (lastTutorialScene.nextScene != null)
                        sceneChanger.nextScene = lastTutorialScene.nextScene;
                    lastTutorialFirstCompletion = false;
                }
            }
        }
    }
}
