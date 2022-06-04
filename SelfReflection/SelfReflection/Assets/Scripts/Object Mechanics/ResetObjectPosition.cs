using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectPosition : MonoBehaviour
{
    private ResetObjectManager manager = null;
    private MoveObjectController playerCam = null;
    private GameObject player = null;
    private GameObject pickUpParent;
    private float time;
    public SkinnedMeshRenderer meshRenderer = null;
    private bool isDead = false;
    private bool isAlive = false;
    private float amt = 0.7f;
    private float curWeight;

    private void Start()
    {
        if(meshRenderer != null)
        {
            meshRenderer.material.shader = Shader.Find("TNTC/Disintegration");
            curWeight = meshRenderer.material.GetFloat("_Weight");
        }
    }

    private void Update()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveObjectController>();
        manager = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ResetObjectManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        pickUpParent = GameObject.Find("PickupParent");

        if (isDead)
        {
            curWeight += amt * Time.deltaTime;
            meshRenderer.material.SetFloat("_Weight", curWeight);
            if(meshRenderer.material.GetFloat("_Weight") > 1)
            {
                meshRenderer.material.SetFloat("_Weight", 1);
                isDead = false;
                ResetPlayer();
            }
        }

        if (isAlive)
        {
            curWeight -= amt * Time.deltaTime;
            meshRenderer.material.SetFloat("_Weight", curWeight);
            if (meshRenderer.material.GetFloat("_Weight") < 0)
            {
                meshRenderer.material.SetFloat("_Weight", 0);
                isAlive = false;
            }
        }
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
                    manager.interactableObj[i].transform.localEulerAngles = manager.objRotation[manager.interactableObj[i]];
                    manager.interactableObj[i].GetComponent<Rigidbody>().freezeRotation = true;
                    if (pickUpParent.transform.childCount != 0)
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
                manager.interactableObj[i].transform.localEulerAngles = manager.objRotation[manager.interactableObj[i]];
                manager.interactableObj[i].GetComponent<Rigidbody>().freezeRotation = true;
                playerCam.DropObject();
            }
        }
        isAlive = true;
    }

    IEnumerator playerSpawn()
    {
        time = 0.1f;
        yield return new WaitForSeconds(time);
        isDead = true;
    }

    IEnumerator itemSpawn(GameObject item)
    {
        time = 0f;
        yield return new WaitForSeconds(time);
        ResetObject(item);
    }

}
