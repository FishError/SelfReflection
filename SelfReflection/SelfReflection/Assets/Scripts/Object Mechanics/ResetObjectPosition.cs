using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectPosition : MonoBehaviour
{
    [Header("References")]
    private ResetObjectManager manager = null;
    public MoveObjectController playerCam = null;
    private GameObject player = null;
    private GameObject pickUpParent;

    private void Update()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveObjectController>();
        manager = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ResetObjectManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (manager.interactableObj.Count == 0)
        {
            return;
        }
        else if(other.gameObject.layer == 7)
        {
            ResetObject();
        }
        else
        {
            ResetPlayer();
        }
    }

    public void ResetObject()
    {
        for (int i = 0; i < manager.interactableObj.Count; i++)
        {
            if (manager.objPosition.ContainsKey(manager.interactableObj[i]))
            {
                int playerIndex = manager.interactableObj.FindIndex(p => p.Equals(player));
                if (i != playerIndex)
                {
                    manager.interactableObj[i].transform.position = manager.objPosition[manager.interactableObj[i]];
                }
                if(playerIndex == -1)
                {
                    manager.interactableObj[i].transform.position = manager.objPosition[manager.interactableObj[i]];
                    playerCam.DropObject();
                }
            }
        }
    }

    public void ResetPlayer()
    {
        for (int i = 0; i < manager.interactableObj.Count; i++)
        {
            pickUpParent = GameObject.Find("PickupParent");
            if (pickUpParent.transform.childCount == 0)
            {
                manager.interactableObj[i].transform.position = manager.objPosition[manager.interactableObj[i]];
            }
            else
            {
                manager.interactableObj[i].transform.position = manager.objPosition[manager.interactableObj[i]];
                playerCam.DropObject();
            }
        }
    }

}
