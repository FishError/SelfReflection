using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageManager : MonoBehaviour
{
    private int count = 0;
    public void trigger()
    {
        count++;
    }
    private void Update()
    {
        if (count >= 2)
        {
            /*SceneManager.LoadScene("");*/
            print("switch scene here");
        }
    }

}
