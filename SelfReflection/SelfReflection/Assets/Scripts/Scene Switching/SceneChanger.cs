using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    public string nextScene;
    private void OnCollisionEnter(Collision collision)
    {
        if (nextScene.Contains("Hub"))
        {
            GameObject hubManager = GameObject.Find("HubManager");
            if (hubManager != null)
            {
                HubManager hm = hubManager.GetComponent<HubManager>();
                hm.SetLevelToCompleted(gameObject.scene.name);
                SceneManager.LoadScene(hm.GetHubSceneToLoad());
                return;
            }
        }

        SceneManager.LoadScene(nextScene);
    }
}
