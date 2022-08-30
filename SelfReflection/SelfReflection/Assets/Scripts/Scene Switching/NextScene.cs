using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NextScene : MonoBehaviour
{

    public void GoToScene(string theScene){
        Debug.Log("Got here");
        SceneManager.LoadScene(sceneName: theScene);
    }
}
