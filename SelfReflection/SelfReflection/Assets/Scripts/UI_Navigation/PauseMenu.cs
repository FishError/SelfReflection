using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject optionMenu;
    public List<GameObject> menus = new List<GameObject>();
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
        if (optionMenu.activeInHierarchy == false)
        {
            optionMenu.SetActive(true);
            isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }

    public void ClosePauseMenu()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].SetActive(false);
        }
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isOpen = 0;
    }

}
