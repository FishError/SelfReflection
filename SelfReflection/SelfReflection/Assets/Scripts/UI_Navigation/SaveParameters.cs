using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveParameters : MonoBehaviour
{
    List<string> keys = new List<string>();
    public GameObject pauseMenu, debugMenu;
    public Slider playerMass, playerJumpForce, playerSpeed, playerClimbSpeed;
    public Text massText, jumpforceText, speedText, climbSpeedText;


    private void Update()
    {
        if(pauseMenu.activeInHierarchy && debugMenu.activeInHierarchy)
        {
            massText = GameObject.Find("MassAmt").GetComponent<Text>();
            jumpforceText = GameObject.Find("JumpAmt").GetComponent<Text>();
            speedText = GameObject.Find("SpeedAmt").GetComponent<Text>();
            climbSpeedText = GameObject.Find("ClimbAmt").GetComponent<Text>();

            if (PlayerPrefs.HasKey("PlayerMass"))
            {
                massText.text = PlayerPrefs.GetFloat("PlayerMass").ToString();
            }

            if (PlayerPrefs.HasKey("PlayerJumpforce"))
            {
                jumpforceText.text = PlayerPrefs.GetFloat("PlayerJumpforce").ToString();
            }

            if (PlayerPrefs.HasKey("PlayerSpeed"))
            {
                speedText.text = PlayerPrefs.GetFloat("PlayerSpeed").ToString();
            }

            if (PlayerPrefs.HasKey("ClimbSpeed"))
            {
                climbSpeedText.text = PlayerPrefs.GetFloat("ClimbSpeed").ToString();
            }
        }
        else
        {
            return;
        }
     
    }

    public void SavePlayerMass(float mass)
    {
        playerMass.value = mass;
        PlayerPrefs.SetFloat("PlayerMass", mass);
        keys.Add("PlayerMass");
    }

    public void SavePlayerJumpForce(float force)
    {
        playerJumpForce.value = force;
        PlayerPrefs.SetFloat("PlayerJumpforce", force);
        keys.Add("PlayerJumpforce");
    }

    public void SavePlayerSpeed(float speed)
    {
        playerSpeed.value = speed;
        PlayerPrefs.SetFloat("PlayerSpeed", speed);
        keys.Add("PlayerSpeed");
    }

    public void SavePlayerClimbSpeed(float climbSpeed)
    {
        playerClimbSpeed.value = climbSpeed;
        PlayerPrefs.SetFloat("ClimbSpeed", climbSpeed);
        keys.Add("ClimbSpeed");
    }

    public void ResetParameters()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            PlayerPrefs.DeleteKey(keys[i]);
        }

        PlayerPrefs.SetFloat("PlayerMass", 1.25f);
        PlayerPrefs.SetFloat("PlayerJumpforce", 12f);
        PlayerPrefs.SetFloat("PlayerSpeed", 7f);
        PlayerPrefs.SetFloat("ClimbSpeed", 3f);
    }
}
