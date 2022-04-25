using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveParameters : MonoBehaviour
{

    public Slider playerMass, playerJumpForce, playerSpeed, playerClimbSpeed;
    public void SavePlayerMass(float mass)
    {
        playerMass.value = mass;
        PlayerPrefs.SetFloat("PlayerMass", mass);
    }

    public void SavePlayerJumpForce(float force)
    {
        playerJumpForce.value = force;
        PlayerPrefs.SetFloat("PlayerJumpforce", force);
    }

    public void SavePlayerSpeed(float speed)
    {
        playerSpeed.value = speed;
        PlayerPrefs.SetFloat("PlayerSpeed", speed);
    }

    public void SavePlayerClimbSpeed(float climbSpeed)
    {
        playerClimbSpeed.value = climbSpeed;
        PlayerPrefs.SetFloat("ClimbSpeed", climbSpeed);
    }
}
