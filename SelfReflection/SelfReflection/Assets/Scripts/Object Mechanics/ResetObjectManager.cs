using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectManager : MonoBehaviour
{
    public Dictionary<GameObject, Vector3> objPosition = new Dictionary<GameObject, Vector3>();
    public List<GameObject> interactableObj = new List<GameObject>();
    public MoveObjectController objectController;

    private void Start()
    {

        if (interactableObj.Count == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < interactableObj.Count; i++)
            {
                objPosition.Add(interactableObj[i], interactableObj[i].transform.position);
            }
        }
    }

    private void Update()
    {
        if(interactableObj.Count == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < interactableObj.Count; i++)
            {

               
            }
        }



    }
}
