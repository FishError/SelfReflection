using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button_Nav : MonoBehaviour
{
    public GameObject oldDisplay;
    public Button button;
    public GameObject newDisplay;
    public int sceneNum = 1;
    // Start is called before the first frame update
    void Start()
    {
        Button myControl = button.GetComponent<Button>();
        myControl.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TaskOnClick()
    {
        if (button.name == "Quit_Button")
        {
            ExitGame();
        }
        else if (button.name == "Start_Button")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+sceneNum);
        }
        else if (button.name == "MainMenu_Button")
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            GameObject control = oldDisplay;
            control.SetActive(false);
            GameObject new_control = newDisplay;
            new_control.SetActive(true);
        }
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
