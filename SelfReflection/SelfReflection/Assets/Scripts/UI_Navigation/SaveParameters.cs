using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveParameters : MonoBehaviour
{
    List<string> keys = new List<string>();
    public Slider playerMass, playerJumpForce, playerSpeed, playerClimbSpeed;
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
        playerMass.value = 1.25f;
        playerJumpForce.value = 12f;
        playerSpeed.value = 7f;
        playerClimbSpeed.value = 3f;

        for (int i = 0; i < keys.Count; i++)
        {
            PlayerPrefs.DeleteKey(keys[i]);
        }
    }
}
