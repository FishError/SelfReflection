using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadParameters : MonoBehaviour
{
    public Slider playerMass, playerJumpForce, playerSpeed, playerClimbSpeed;
    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (PlayerPrefs.HasKey("PlayerMass"))
        {
            playerMass.value = PlayerPrefs.GetFloat("PlayerMass");
            player.GetComponent<Rigidbody>().mass = PlayerPrefs.GetFloat("PlayerMass", 1.25f);
        }

        if (PlayerPrefs.HasKey("PlayerJumpforce"))
        {
            playerJumpForce.value = PlayerPrefs.GetFloat("PlayerJumpforce");
            player.GetComponent<PlayerMovement>().jumpForce = PlayerPrefs.GetFloat("PlayerJumpforce", 12f);
        }

        if (PlayerPrefs.HasKey("PlayerSpeed"))
        {
            playerSpeed.value = PlayerPrefs.GetFloat("PlayerSpeed");
            player.GetComponent<PlayerMovement>().moveSpeed = PlayerPrefs.GetFloat("PlayerSpeed", 7f);
        }

        if (PlayerPrefs.HasKey("ClimbSpeed"))
        {
            playerClimbSpeed.value = PlayerPrefs.GetFloat("ClimbSpeed");
            player.GetComponent<PlayerMovement>().climbUpSpeed = PlayerPrefs.GetFloat("ClimbSpeed", 3f);
        }
    }
}
