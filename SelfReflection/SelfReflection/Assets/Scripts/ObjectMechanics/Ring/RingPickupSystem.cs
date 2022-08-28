using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class RingPickupSystem : MonoBehaviour
{
    public GameObject ringCam;
    public GameObject PlayerCam;
    public string nextScene;
    public PostProcessVolume postProcessVolume;
    private Animator anim;
    private PlayerMovement playerMovement;
    private GameObject player;
    private AutoExposure exposureSetting;
    private bool startFlicker=false;
    public GameObject audioObject;
    public GameObject backgroundAudioObject;

    private float timeSinceLastFlicker;
    private float timeSinceSequenceStart;
    private bool isSequenceStarted;

    [SerializeField]
    private FMODUnity.StudioEventEmitter ringDialogue;
    private void Start()
    {
        anim = this.transform.parent.GetComponent<Animator>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        player = GameObject.Find("Player");
        exposureSetting = postProcessVolume.profile.GetSetting<AutoExposure>();
    }

    private void Update()
    {
        if (isSequenceStarted)
        {
            Destroy(backgroundAudioObject);
            
            //Approx 28 seconds for ring dialogue to finish
            if (timeSinceSequenceStart > 26f)
            {
                ringCam.SetActive(false);
                flickrLight();
                audioObject.SetActive(true);
            }

            //After Damien Call End
            if (timeSinceSequenceStart > 44f)
            {
                Debug.Log("Starting flicker and camera shake.");
                startFlicker=true;
                PlayerCam.GetComponent<CameraShake>().shakeDuration = 10.0f;
            }

            //Revert Things To Normal After Sequence Finish
            if (timeSinceSequenceStart > 60f)
            {
                anim.SetBool("isTouching", false);
                playerMovement.EnableMovement();
                this.transform.parent.localEulerAngles = new Vector3(0, 90, 0);
                player.SetActive(true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
                isSequenceStarted = false;
                PlayerCam.GetComponent<CameraShake>().shakeDuration = 0.0f;
                exposureSetting.enabled.Override(false);
                SceneManager.LoadScene(sceneName: nextScene);
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

    private void flickrLight()
    {
        exposureSetting.enabled.Override(true);
        exposureSetting.keyValue.Override(0.5f);
        if (timeSinceLastFlicker > 0.10f && startFlicker==true)
        {
            exposureSetting.keyValue.Override(Random.Range(0.04f, 0.24f));
            timeSinceLastFlicker = 0;
        }
        timeSinceLastFlicker += Time.deltaTime;
    }
}
