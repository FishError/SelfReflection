using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Rigidbody wall;

    private void OnTriggerEnter(Collider other)
    {
        gameObject.transform.position = new Vector3(10, (float)-0.24, -10);

        wall.isKinematic = false;
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.transform.position = new Vector3(10, 0, -10);
    }
}
