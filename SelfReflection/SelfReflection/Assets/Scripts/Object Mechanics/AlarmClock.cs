using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AlarmClock : MonoBehaviour
{
    public Text text;

    [Header("Edit Time (in Seconds)")]
    public float timer = 0.0f;
    public bool timerIsRunning = false;

    private void Start()
    {
        text.text = "Pause";
        StartCoroutine(wait());
    }


    private void Update()
    {
        if (timerIsRunning)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                DisplayTime(timer);
            }
            else
            {
                print("Ring Ring Ring");
                timer = 0f;
                timerIsRunning = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        
    }

    public void DisplayTime(float timer)
    {
        timer += 1;
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60);
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        timerIsRunning = true;
    }
}
