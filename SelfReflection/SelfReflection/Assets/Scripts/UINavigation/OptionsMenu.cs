using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public int menuState = 0;
    public bool isPaused;
    public GameObject lastPage;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenOptionsMenu();
        }

        if (isPaused)
        {
            CloseOptionsMenu();
        }
    }

    void OpenOptionsMenu()
    {
        if (optionsMenu.activeInHierarchy == false)
        {
            optionsMenu.SetActive(true);
            isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }

    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void SetNewPage(GameObject page)
    {
        if(lastPage != null)
        {
            lastPage.SetActive(false);
            page.SetActive(true);

        }
        else
        {
            page.SetActive(true);
        }
        lastPage = page;

    }

    public void quitGame()
    {
        Application.Quit();
    }

}
