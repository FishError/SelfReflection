using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEtherealManager : MonoBehaviour
{
    [Header("List of Interactable Ethereal Objects")]
    public List<GameObject> etherealObj = new List<GameObject>();
    public Dictionary<GameObject, Vector3> etherealPos = new Dictionary<GameObject, Vector3>();
    public Dictionary<GameObject, Vector3> etherealRot = new Dictionary<GameObject, Vector3>();

    private void Start()
    {
        if (etherealObj.Count == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < etherealObj.Count; i++)
            {
                etherealPos.Add(etherealObj[i], etherealObj[i].transform.position);
                etherealRot.Add(etherealObj[i], etherealObj[i].transform.localEulerAngles);
            }
        }
    }
}
