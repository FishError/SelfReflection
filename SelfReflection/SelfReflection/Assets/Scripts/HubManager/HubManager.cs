using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class sceneHubObjects
{
    public string sceneName;
    public bool completed;
    public GameObject ParentOfObjects;
    public string parentOfObjectsName;
}

public class HubManager : MonoBehaviour
{
    public List<sceneHubObjects> scenes;

    public string hubName;

    // Start is called before the first frame update
    void Start()
    {
        hubName = gameObject.scene.name;
        SceneManager.activeSceneChanged += ActiveSceneChanged;

        foreach (sceneHubObjects scene in scenes)
        {
            scene.parentOfObjectsName = scene.ParentOfObjects.name;
        }
    }

    public void SetSceneToCompleted(string sceneName)
    {
        var scene = scenes.Find(s => s.sceneName == sceneName);
        scene.completed = true;
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        if (next.name == hubName)
        {
            foreach (sceneHubObjects scene in scenes)
            {
                if (scene.completed)
                {
                    var obj = GameObject.Find(scene.parentOfObjectsName);
                    foreach (Transform t in obj.transform)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
