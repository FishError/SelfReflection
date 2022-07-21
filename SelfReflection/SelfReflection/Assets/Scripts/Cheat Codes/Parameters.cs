using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Parameters : MonoBehaviour
{
    private GameObject player;
    public GameObject debugMenu;
    public List<GameObject> param = new List<GameObject>();
    public Dictionary<string, float> defaultVal = new Dictionary<string, float>();

    private void Start()
    {
        //Start Debug Menu in an inactive state
        debugMenu.SetActive(false);
        Time.timeScale = 1;

        //Setting up default values for each parameter
        defaultVal.Add("mass", 1.25f);
        defaultVal.Add("jumpforce", 12f);
        defaultVal.Add("speed", 7f);
        defaultVal.Add("climbspeed", 12f);

        //Find the player component to get its PlayerMovement component
        player = GameObject.Find("Player");

        //Load on Start: If there exist a key for a parameter, apply the effect of that parameter to the player
        for (int i = 0; i < param.Count; i++)
        {
            if (PlayerPrefs.HasKey(param[i].transform.GetChild(1).name))
            {
                switch (param[i].transform.GetChild(1).name)
                {
                    case "mass":
                        player.GetComponent<Rigidbody>().mass = PlayerPrefs.GetFloat(param[i].transform.GetChild(1).name, 1.25f);
                        break;
                    case "jumpforce":
                        player.GetComponent<PlayerMovement>().jumpForce = PlayerPrefs.GetFloat(param[i].transform.GetChild(1).name, 12f);
                        break;
                    case "speed":
                        player.GetComponent<PlayerMovement>().moveSpeed = PlayerPrefs.GetFloat(param[i].transform.GetChild(1).name, 7f);
                        break;
                    case "climbspeed":
                        player.GetComponent<PlayerMovement>().climbUpSpeed = PlayerPrefs.GetFloat(param[i].transform.GetChild(1).name, 12f);
                        break;
                }
            }
            else
            {
                return;
            }
        }

        //Load on Start: Update the slider value and the text based on the last saved value
        for (int i = 0; i < param.Count; i++)
        {
            param[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = PlayerPrefs.GetFloat(param[i].transform.GetChild(1).name).ToString("F2");
            param[i].transform.GetChild(0).gameObject.GetComponent<Slider>().value = PlayerPrefs.GetFloat(param[i].transform.GetChild(1).name);
        }
    }

    //Update the text value based on the slider value
    private void Update()
    {
        for (int i = 0; i < param.Count; i++)
        {
            param[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = PlayerPrefs.GetFloat(param[i].transform.GetChild(1).name).ToString("F2");
        }
    }

    //Change player mass
    public void playerMass(float mass)
    {
        player.GetComponent<Rigidbody>().mass = mass;
        PlayerPrefs.SetFloat("mass", mass);
    }

    //Change player jump force
    public void playerJumpForce(float force)
    {
        player.GetComponent<PlayerMovement>().jumpForce = force;
        PlayerPrefs.SetFloat("jumpforce", force);
    }

    //Change player speed
    public void playerSpeed(float speed)
    {
        player.GetComponent<PlayerMovement>().moveSpeed = speed;
        PlayerPrefs.SetFloat("speed", speed);
    }

    //Change player climb speed
    public void playerClimbSpeed(float climbSpeed)
    {
        player.GetComponent<PlayerMovement>().climbUpSpeed = climbSpeed;
        PlayerPrefs.SetFloat("climbspeed", climbSpeed);
    }

    //Reset current parameter values to default values
    public void resetDebugManager()
    {
        player.GetComponent<Rigidbody>().mass = defaultVal["mass"];
        player.GetComponent<PlayerMovement>().jumpForce = defaultVal["jumpforce"];
        player.GetComponent<PlayerMovement>().moveSpeed = defaultVal["speed"];
        player.GetComponent<PlayerMovement>().climbUpSpeed = defaultVal["climbspeed"];
        
        for (int i = 0; i < param.Count; i++)
        {
            param[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = defaultVal[param[i].transform.GetChild(1).name].ToString();
            param[i].transform.GetChild(0).gameObject.GetComponent<Slider>().value = defaultVal[param[i].transform.GetChild(1).name];
        }
    }

    //Change scenes based on specified scene name
    public void changeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }


}
