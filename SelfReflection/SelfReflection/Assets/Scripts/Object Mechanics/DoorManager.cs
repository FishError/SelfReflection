using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public GameObject door;
    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {

        if (!used & other.tag == "Player")
        {
            if (door.activeSelf)
            {
                door.SetActive(false);
            }
            else
            {
                door.SetActive(true);
            }
            used = true;
        }
    }
}
