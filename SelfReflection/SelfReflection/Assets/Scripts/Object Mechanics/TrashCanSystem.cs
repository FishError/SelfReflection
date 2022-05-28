using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashCanSystem : MonoBehaviour
{
    public Vector3 lidRotation;
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

        int trashIndex = objectToDisable.FindIndex(t => t.Equals(other.gameObject));
        if (trashIndex != -1)
        {
            print("here");
            objectToDisable[countObjectThrown].SetActive(false);
            countObjectThrown++;

            if (countObjectThrown == NumOfObjectToThrow)
            {
                lid.transform.localEulerAngles = lidRotation;
                lid.gameObject.layer = finalLayer;
                isCovered = true;
            }
        }
    }
}
