using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public Canvas optionsCanvas;
    public int menuState = 0;
    public bool isPaused;
    public GameObject lastPage;

    // Start is called before the first frame update
    void Start()
    {
        optionsMenu = GetComponent<GameObject>();
        optionsCanvas = optionsMenu.GetComponent<Canvas>();
    }

    // Update is called once per frame
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
        page.SetActive(true);
        lastPage = page;
    }

}
