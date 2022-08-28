using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FragmentTransitionNext : MonoBehaviour
{
    public string nextScene;

    public void moveToNextStage()
    {
        SceneManager.LoadScene(sceneName: nextScene);

    }
}
