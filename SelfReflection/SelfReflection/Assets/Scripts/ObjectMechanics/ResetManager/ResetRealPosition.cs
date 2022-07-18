using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRealPosition : MonoBehaviour
{
    private ResetRealManager realManager;
    private InteractionController playerCam = null;


    private void Start()
    {
        realManager = GameObject.Find("RealManager").GetComponent<ResetRealManager>();
    }

    private void Update()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<InteractionController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        int objIndex = realManager.realObj.FindIndex(p => p.Equals(this.gameObject));
        if (other.tag == "Real Barrier" && objIndex != -1 && this.gameObject.tag == "Real")
        {
            for (int i = 0; i < realManager.realObj.Count; i++)
            {
                if (realManager.realPos.ContainsKey(realManager.realObj[i]))
                {
                    realManager.realObj[i].transform.position = realManager.realPos[realManager.realObj[i]];
                    realManager.realObj[i].transform.localEulerAngles = realManager.realRot[realManager.realObj[i]];
                    realManager.realObj[i].GetComponent<Rigidbody>().freezeRotation = true;
                    if (realManager.realObj[i].GetComponent<InteractableObject>().state == ObjectState.MovingThroughMirror)
                    {
                        playerCam.DropObject();
                    }
                }
            }
        }
    }
}
