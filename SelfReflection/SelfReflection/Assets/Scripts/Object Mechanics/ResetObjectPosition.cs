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
    private float time;

    private void Update()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveObjectController>();
        manager = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ResetObjectManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        pickUpParent = GameObject.Find("PickupParent");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (manager.interactableObj.Count == 0)
        {
            return;
        }
        else if(this.gameObject.layer == 7 && other.tag == "Void") //Respawn Objects (excluding Player) when dropped into the Void
        {
            StartCoroutine(itemSpawn(this.gameObject));
        }

        else if(pickUpParent.transform.childCount != 0 && other.tag == "Void" && this.tag == "Player") //Respawn Objects and Player if Player is holding the object and dropping into the Void
        {
            StartCoroutine(playerSpawn());
        }
        else if (pickUpParent.transform.childCount == 0 && other.tag == "Void" && this.tag == "Player") //Respawn Player
        {
            StartCoroutine(playerSpawn());
        }
    }

    public void ResetObject(GameObject item)
    {
        for (int i = 0; i < manager.interactableObj.Count; i++)
        {
            if (manager.objPosition.ContainsKey(manager.interactableObj[i]))
            {
                int playerIndex = manager.interactableObj.FindIndex(p => p.Equals(player));
                int itemIndex = manager.interactableObj.FindIndex(p => p.Equals(item));
                if (i != playerIndex && i == itemIndex)
                {
                    manager.interactableObj[i].transform.position = manager.objPosition[manager.interactableObj[i]];
                    if(pickUpParent.transform.childCount != 0)
                    {
                        playerCam.DropObject();
                    }
                }
            }
            
        }
    }

    public void ResetPlayer()
    {
        for (int i = 0; i < manager.interactableObj.Count; i++)
        {
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

    IEnumerator playerSpawn()
    {
        time = 1.5f;
        yield return new WaitForSeconds(time);
        print("Player is Dead");
        ResetPlayer();
        
    }

    IEnumerator itemSpawn(GameObject item)
    {
        time = 0f;
        yield return new WaitForSeconds(time);
        print("Item Spawning");
        ResetObject(item);
    }

}
