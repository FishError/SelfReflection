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

        // If there is no Save file, continue button should not be interactable
        if (button.name == "Continue_Button" && !SaveSystem.HasSaveFile)
        {
            button.interactable = false;
            GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
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
            // Start button should go to demo stage 1 by default
            SceneManager.LoadScene((int) SceneIndex.DemoStage1);
            Time.timeScale = 1;
        }
        else if (button.name == "Continue_Button")
        {
            // Safe-guarding
            if (!SaveSystem.HasSaveFile)
            {
                Debug.LogError("The file Saves.json does not exist.");
                return;
            }

            // Continue button should go to the last visited level
            int nextSceneIndex = (int) SaveSystem.Load().LastVisitedLevel;
            SceneManager.LoadScene(nextSceneIndex);
            Time.timeScale = 1;
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