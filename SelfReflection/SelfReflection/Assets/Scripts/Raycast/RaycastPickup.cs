using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    private GameObject heldObject;
    private Rigidbody heldObjectrb;

    [Header("Physics Parameters")]
    public RaycastHit hit;
    private Ray ray;
    public float maxDistance = 150f;
    public int reflections;
    private float mouseX, mouseY;
    public float sensX;
    public float sensY;


    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        if (Input.GetMouseButtonDown(0))
        {
            //print("Left Click");
            if (heldObject == null)
            {
                ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
                for (int i = 0; i < reflections; i++)
                {
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, maxDistance))
                    {
                        ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                        if (hit.collider.CompareTag("Real") && i > 0)
                        {
                            PickupObject(hit.transform.gameObject);
                        }
                        else if (hit.collider.CompareTag("Ethereal"))
                        {
                            PickupObject(hit.transform.gameObject);
                        }
                        if (hit.collider.tag != "Mirror")
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                DropObject();
            }
        }
        if (heldObject != null)
        {
            heldObjectrb.transform.position = new Vector3(heldObjectrb.transform.position.x, heldObjectrb.transform.position.y + mouseY, heldObjectrb.transform.position.z + mouseX);
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