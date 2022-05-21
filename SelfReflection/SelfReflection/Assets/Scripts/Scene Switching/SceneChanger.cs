using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    public string nextScene;
    private void OnCollisionEnter(Collision collision)
    {
        GameObject hubManager = GameObject.Find("HubManager");
        if (hubManager)
        {
            hubManager.GetComponent<HubManager>().SetSceneToCompleted(gameObject.scene.name);
        }

        SceneManager.LoadScene(nextScene);
    }
}
