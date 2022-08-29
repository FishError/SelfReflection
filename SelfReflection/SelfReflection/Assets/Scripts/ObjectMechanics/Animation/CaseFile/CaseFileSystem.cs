using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaseFileSystem : MonoBehaviour
{
    public GameObject fileCam;
    public string nextScene;
    private Animator anim;
    private PlayerMovement playerMovement;
    private GameObject player;
    private PauseMenu pauseMenu;

    private void Start()
    {
        anim = this.transform.parent.GetComponent<Animator>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        player = GameObject.Find("Player");
        pauseMenu = GameObject.Find("GameManager").GetComponent<PauseMenu>();
    }

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("CaseFile_Open") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            fileCam.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            fileCam.SetActive(true);
            anim.SetBool("isTouching", true);
            playerMovement.DisableMovement();
            this.transform.parent.localEulerAngles = new Vector3(0, 90, 0);
            player.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            pauseMenu.GetComponent<PauseMenu>().enabled = false;
        }
    }

    public void CloseFile()
    {
        if (nextScene.Contains("Hub"))
        {
            GameObject hubManager = GameObject.Find("HubManager");
            if (hubManager != null)
            {
                HubManager hm = hubManager.GetComponent<HubManager>();
                hm.SetLevelToCompleted(gameObject.scene.name);
                SceneManager.LoadScene(hm.GetHubSceneToLoad());
                return;
            }
        }
            
        SceneManager.LoadScene(sceneName: nextScene);
    }
}
