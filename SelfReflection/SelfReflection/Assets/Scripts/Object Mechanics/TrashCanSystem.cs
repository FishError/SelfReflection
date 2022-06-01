using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashCanSystem : MonoBehaviour
{
    public Vector3 lidRotation;
    public GameObject lid;
    public GameObject Book;
    public int initLayer, finalLayer;
    public bool isCovered;
    public List<GameObject> objectToDisable;
    public int NumOfObjectToThrow = 0;
    private int countObjectThrown;
    public float theTime=0;

    private void Start()
    {
        isCovered = false;
        lid.gameObject.layer = initLayer;
        countObjectThrown = 0;
    }
    private void Update(){
        if(isCovered){
            theTime+=Time.deltaTime;
            if(theTime>=6f){
                Book.GetComponent<Rigidbody>().useGravity = false;
                Book.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
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
            objectToDisable[trashIndex].SetActive(false);
            countObjectThrown++;

            if (countObjectThrown == NumOfObjectToThrow)
            {
                lid.transform.localEulerAngles = lidRotation;
                lid.gameObject.layer = finalLayer;
                isCovered = true;
                Book.GetComponent<Rigidbody>().useGravity = true;
                Vector3 m_NewForce = new Vector3(-40000f, 0f, 0f);
                Book.GetComponent<Rigidbody>().AddForce(m_NewForce);
                Book.GetComponent<InteractableObject>().state=ObjectState.InteractionDisabled;
                Debug.Log("Made It Here");
            }
        }
    }
}
