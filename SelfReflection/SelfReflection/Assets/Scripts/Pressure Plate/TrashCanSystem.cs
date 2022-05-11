using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashCanSystem : MonoBehaviour
{
    public Vector3 lidPosition;
    public Vector3 lidRotation;
    public Vector3 lidScale;
    public GameObject lid;
    public int initLayer, finalLayer;
    public bool isCovered;
    public List<GameObject> objectToDisable;
    public int NumOfObjectToThrow = 0;
    private int countObjectThrown;

    private void Start()
    {
        isCovered = false;
        lid.gameObject.layer = initLayer;
        countObjectThrown = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isCovered == false)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if(other.gameObject.name.Contains("ThrowObject"))
        {
            objectToDisable[countObjectThrown].SetActive(false);
            countObjectThrown++;

            if (countObjectThrown == NumOfObjectToThrow)
            {
                lid.transform.localPosition = lidPosition;
                lid.transform.localEulerAngles = lidRotation;
                lid.transform.localScale = lidScale;
                lid.gameObject.layer = finalLayer;
                isCovered = true;
            }
        }
    }
}
