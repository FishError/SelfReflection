using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CaseFileSystem : MonoBehaviour
{
    public GameObject fileCam;
    public TextMeshProUGUI text;
    public GameObject alarmClockCanvas;
    private Animator anim;
    private PlayerMovement playerMovement;
    private GameObject player;

    private void Start()
    {
        anim = this.transform.parent.GetComponent<Animator>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        player = GameObject.Find("Player");
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
            alarmClockCanvas.GetComponent<AlarmClock>().timerIsRunning = false;
            alarmClockCanvas.transform.GetChild(1).gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
