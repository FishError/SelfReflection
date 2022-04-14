using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Rigidbody wall;
    private float sinkAmount = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" || gameObject.tag == "Interactable")
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - sinkAmount, gameObject.transform.position.z);
            wall.isKinematic = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player" || gameObject.tag == "Interactable")
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + sinkAmount, gameObject.transform.position.z);
        }
    }
}
