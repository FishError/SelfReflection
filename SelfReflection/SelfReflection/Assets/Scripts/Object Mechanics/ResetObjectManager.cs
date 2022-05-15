using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectManager : MonoBehaviour
{
    public GameObject northWall, southWall, leftWall, rightWall;
    public Dictionary<GameObject, Vector3> objPosition = new Dictionary<GameObject, Vector3>();
    public List<GameObject> interactableObj = new List<GameObject>();
    public MoveObjectController objectController;

    private void Start()
    {
        if(interactableObj.Count == 0)
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

                /*
                CheckLeftWall(i);
                CheckRightWall(i);
                CheckNorthWall(i);
                CheckSouthWall(i);*/
            }
        }
        
    }


    /*
    public void CheckLeftWall(int i)
    {
        if (interactableObj[i].transform.position.x < leftWall.transform.position.x)
        {
            if (objPosition.ContainsKey(interactableObj[i]))
            {
                interactableObj[i].transform.position = objPosition[interactableObj[i]];
                objectController.DropObject();
            }
        }
    }

    public void CheckRightWall(int i)
    {
        if (interactableObj[i].transform.position.x > rightWall.transform.position.x)
        {
            if (objPosition.ContainsKey(interactableObj[i]))
            {
                interactableObj[i].transform.position = objPosition[interactableObj[i]];
                objectController.DropObject();
            }
        }
    }

    public void CheckNorthWall(int i)
    {
        if (interactableObj[i].transform.position.z > northWall.transform.position.z)
        {
            if (objPosition.ContainsKey(interactableObj[i]))
            {
                interactableObj[i].transform.position = objPosition[interactableObj[i]];
                objectController.DropObject();
            }
        }
    }

    public void CheckSouthWall(int i)
    {
        if (interactableObj[i].transform.position.z < southWall.transform.position.z)
        {
            if (objPosition.ContainsKey(interactableObj[i]))
            {
                interactableObj[i].transform.position = objPosition[interactableObj[i]];
                objectController.DropObject();
            }
        }
    }*/
}
