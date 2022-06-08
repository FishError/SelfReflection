using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class BodyBagPickupSystem : MonoBehaviour
{
    public GameObject ringCam;
    public GameObject PlayerCam;
    public string nextScene;
    private Animator anim;
    private PlayerMovement playerMovement;
    private GameObject player;

    private float timeSinceSequenceStart;
    private bool isSequenceStarted;

    [SerializeField]
    private FMODUnity.StudioEventEmitter ringDialogue;
    private void Start()
    {
        anim = this.transform.parent.GetComponent<Animator>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (isSequenceStarted)
        {
            
            //Approx 14 seconds for bodybag dialogue to finish
            if (timeSinceSequenceStart > 14f)
            {
                ringCam.SetActive(false);
                anim.SetBool("isTouching", false);
                playerMovement.EnableMovement();
                this.transform.parent.localEulerAngles = new Vector3(0, 90, 0);
                player.SetActive(true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
                isSequenceStarted = false;
                CloseFile();
            }

            timeSinceSequenceStart += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ringCam.SetActive(true);
            ringCam.transform.GetChild(0).gameObject.SetActive(true);
            ringCam.transform.GetChild(1).gameObject.SetActive(true);
            anim.SetBool("isTouching", true);
            playerMovement.DisableMovement();
            this.transform.parent.localEulerAngles = new Vector3(0, 90, 0);
            player.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isSequenceStarted = true;
        }
    }

   private void CloseFile()
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
