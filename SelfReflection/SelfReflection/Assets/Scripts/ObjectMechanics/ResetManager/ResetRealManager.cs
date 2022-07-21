using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRealManager : MonoBehaviour
{
    [Header("List of Interactable Real Objects")]
    public List<GameObject> realObj = new List<GameObject>();
    public Dictionary<GameObject, Vector3> realPos = new Dictionary<GameObject, Vector3>();
    public Dictionary<GameObject, Vector3> realRot = new Dictionary<GameObject, Vector3>();

    private void Start()
    {
        if (realObj.Count == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < realObj.Count; i++)
            {
                realPos.Add(realObj[i], realObj[i].transform.position);
                realRot.Add(realObj[i], realObj[i].transform.localEulerAngles);
            }

        }
    }
}
