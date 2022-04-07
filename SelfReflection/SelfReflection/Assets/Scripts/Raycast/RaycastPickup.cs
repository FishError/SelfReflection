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
    public RaycastHit hit;
    private Ray ray;
    private float remainingLength = 1000;
    private float mouseX, mouseY;
    public float sensX;
    public float sensY;


    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
                for (int i = 0; i < 5; i++)
                {
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
                    {
                        remainingLength -= Vector3.Distance(ray.origin, hit.point);
                        ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                        if (hit.collider.CompareTag("Cube"))
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
            heldObjectrb.drag = 10;
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
