using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectManager : MonoBehaviour
{
    [Header("List of Interactable Objects")]
    public List<GameObject> interactableObj = new List<GameObject>();
    public Dictionary<GameObject, Vector3> objPosition = new Dictionary<GameObject, Vector3>();
    public Dictionary<GameObject, Vector3> objRotation = new Dictionary<GameObject, Vector3>();

    [Header("List of Interactable Ethereal Objects")]
    public List<GameObject> etherealObj = new List<GameObject>();
    public Dictionary<GameObject, Vector3> etherealPos = new Dictionary<GameObject, Vector3>();
    public Dictionary<GameObject, Vector3> etherealRot = new Dictionary<GameObject, Vector3>();

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
                objRotation.Add(interactableObj[i], interactableObj[i].transform.localEulerAngles);
            }
        }
    }

    private void Update()
    {
        
    }
}
