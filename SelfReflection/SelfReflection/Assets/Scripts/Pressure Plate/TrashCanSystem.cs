using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashCanSystem : MonoBehaviour
{
    public Vector3 lidPosition;
    public Vector3 lidRotation;
    public GameObject lid;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if(other.gameObject.name == "ThrowObject")
        {
            lid.transform.localPosition = lidPosition;
            lid.transform.localEulerAngles = lidRotation;
        }
    }
}
