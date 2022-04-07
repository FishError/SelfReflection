using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObject;
    private Rigidbody heldObjectrb;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(heldObject == null)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    //Pick Up Object
                    print("Hit");
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                //Drop Object
                DropObject();
            }
        }
    }


    void PickupObject(GameObject obj)
    {
        if (obj.GetComponent<Rigidbody>())
        {
            heldObjectrb = obj.GetComponent<Rigidbody>();
            heldObjectrb.useGravity = false;
            heldObjectrb.drag = 5;
            heldObjectrb.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectrb.transform.parent = holdArea;
            heldObject = obj;
        }
    }

    void DropObject()
    {
        heldObjectrb.useGravity = true;
        heldObjectrb.drag = 1;
        heldObjectrb.constraints = RigidbodyConstraints.None;

        heldObject.transform.parent = null;
        heldObject = null;
    }

}
