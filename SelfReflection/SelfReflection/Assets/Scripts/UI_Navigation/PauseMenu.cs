using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    private int isOpen = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen++;
            OpenPauseMenu();
        }
        
        if(isOpen == 2)
        {
            ClosePauseMenu();
        }
        
    }

    void OpenPauseMenu()
    {
        if (pauseMenu.activeInHierarchy == false)
        {
            pauseMenu.SetActive(true);
            isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isOpen = 0;
    }

}
