using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    public string nextScene;
    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene(nextScene);
    }
}
