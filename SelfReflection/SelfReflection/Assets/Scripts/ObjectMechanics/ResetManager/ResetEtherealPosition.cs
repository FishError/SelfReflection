using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEtherealPosition : MonoBehaviour
{
    private ResetEtherealManager etherealManager;
    private GameObject pickUpParent;
    private MoveObjectController playerCam = null;


    private void Start()
    {
        etherealManager = GameObject.Find("EtherealManager").GetComponent<ResetEtherealManager>();
        pickUpParent = GameObject.Find("PickupParent");
    }

    private void Update()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveObjectController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        int objIndex = etherealManager.etherealObj.FindIndex(p => p.Equals(this.gameObject));
        if (other.tag == "Ethereal Barrier" && objIndex != -1 && this.gameObject.tag == "Ethereal")
        {
            for (int i = 0; i < etherealManager.etherealObj.Count; i++)
            {
                if (etherealManager.etherealPos.ContainsKey(etherealManager.etherealObj[i]))
                {
                    etherealManager.etherealObj[i].transform.position = etherealManager.etherealPos[etherealManager.etherealObj[i]];
                    etherealManager.etherealObj[i].transform.localEulerAngles = etherealManager.etherealRot[etherealManager.etherealObj[i]];
                    etherealManager.etherealObj[i].GetComponent<Rigidbody>().freezeRotation = true;
                    if (pickUpParent.transform.childCount != 0 || etherealManager.etherealObj[i].GetComponent<InteractableObject>().state == ObjectState.MovingThroughMirror)
                    {
                        playerCam.DropObject();
                    }
                }

            }
        }
    }
}
