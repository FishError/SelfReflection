using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parameters : MonoBehaviour
{
    private GameObject player;
    public List<GameObject> param = new List<GameObject>();
    public Dictionary<string, float> defaultVal = new Dictionary<string, float>();

    private void Start()
    {
        player = GameObject.Find("Player");
        for (int i = 0; i < param.Count; i++)
        {
            if (PlayerPrefs.HasKey(param[i].transform.GetChild(1).name))
            {
                switch (param[i].transform.GetChild(1).name)
                {
                    case "mass":
                        player.GetComponent<Rigidbody>().mass = PlayerPrefs.GetInt(param[i].transform.GetChild(1).name);
                        break;
                    case "jumpforce":
                        player.GetComponent<PlayerMovement>().jumpForce = PlayerPrefs.GetInt(param[i].transform.GetChild(1).name);
                        break;
                    case "speed":
                        player.GetComponent<PlayerMovement>().moveSpeed = PlayerPrefs.GetInt(param[i].transform.GetChild(1).name);
                        break;
                    case "climbspeed":
                        player.GetComponent<PlayerMovement>().climbUpSpeed = PlayerPrefs.GetInt(param[i].transform.GetChild(1).name);
                        break;
                }
            }
            else
            {
                return;
            }
        }
    }

    public void playerMass(float mass)
    {
        player.GetComponent<Rigidbody>().mass = mass;
        GameObject.Find("mass").GetComponent<Text>().text = Mathf.RoundToInt(mass).ToString();
        PlayerPrefs.SetInt("mass", Mathf.RoundToInt(mass));
    }

    public void playerJumpForce(float force)
    {
        player.GetComponent<PlayerMovement>().jumpForce = force;
        GameObject.Find("jumpforce").GetComponent<Text>().text = Mathf.RoundToInt(force).ToString();
        PlayerPrefs.SetInt("jumpforce", Mathf.RoundToInt(force));
    }

    public void playerSpeed(float speed)
    {
        player.GetComponent<PlayerMovement>().moveSpeed = speed;
        GameObject.Find("speed").GetComponent<Text>().text = Mathf.RoundToInt(speed).ToString();
        PlayerPrefs.SetInt("speed", Mathf.RoundToInt(speed));
    }

    public void playerClimbSpeed(float climbSpeed)
    {
        player.GetComponent<PlayerMovement>().climbUpSpeed = climbSpeed;
        GameObject.Find("climbspeed").GetComponent<Text>().text = Mathf.RoundToInt(climbSpeed).ToString();
        PlayerPrefs.SetInt("climbspeed", Mathf.RoundToInt(climbSpeed));
    }
}
