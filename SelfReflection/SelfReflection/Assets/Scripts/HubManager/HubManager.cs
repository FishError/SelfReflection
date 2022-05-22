using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelHubObjects
{
    public string sceneName;
    public bool completed;
    public GameObject portalMirror;
    public GameObject parentOfObjects;

    public static GameObject portalMirrorReference { get; set; }
    public static GameObject parentOfObjectsReference { get; set; }

    [HideInInspector]
    public string portalMirrorName;
    [HideInInspector]
    public string parentOfObjectsName;
}

public class HubManager : MonoBehaviour
{
    public string hubName;
    public List<LevelHubObjects> scenes;
    public List<string> hubCompletionOrder;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += ActiveSceneChanged;

        foreach (LevelHubObjects scene in scenes)
        {
            scene.parentOfObjectsName = scene.parentOfObjects.name;
            scene.portalMirrorName = scene.portalMirror.name;
        }
    }

    public void SetLevelToCompleted(string sceneName)
    {
        var scene = scenes.Find(s => s.sceneName == sceneName);
        if (scene != null)
            scene.completed = true;
    }

    public string GetHubSceneToLoad()
    {
        var numCompleted = scenes.Count(s => s.completed);
        return hubCompletionOrder[numCompleted];
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        if (next.name.Contains(hubName))
        {
            foreach (LevelHubObjects scene in scenes)
            {
                if (scene.completed)
                {
                    var obj = GameObject.Find(scene.parentOfObjectsName);
                    print(obj);
                    foreach (Transform t in obj.transform)
                    {
                        t.gameObject.SetActive(true);
                    }
                    GameObject.Find(scene.portalMirrorName).SetActive(false);
                }
            }
        }
    }
}
