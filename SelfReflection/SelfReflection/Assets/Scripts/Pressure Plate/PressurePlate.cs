using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Rigidbody wall;
    public float sinkAmount;

    private void OnTriggerEnter(Collider other)
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - sinkAmount, gameObject.transform.position.z);

        wall.isKinematic = false;
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - sinkAmount, gameObject.transform.position.z);
    }
}
