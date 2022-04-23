using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    private InteractableObject interactableObject;
    private Rigidbody interactableObjectRB;
    public int interactableLayerIndex;

    [Header("Physics Parameters")]
    public RaycastHit hit;
    private Ray ray;
    public float maxDistance = 150f;
    public int reflections;
    private float mouseX, mouseY, mouseScroll;
    private float sensX;
    private float sensY;
    public float mouseScrollSense;
    public float objectMoveSpeed = 1;

    private Transform relativeMirror;

    private void Start()
    {
        sensX = transform.GetComponent<PlayerCam>().sensX;
        sensY = transform.GetComponent<PlayerCam>().sensY;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //print("Left Click");
            if (interactableObject == null)
            {
                ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
                for (int i = 0; i < reflections; i++)
                {
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, maxDistance))
                    {
                        if (hit.collider.transform.gameObject.layer == interactableLayerIndex)
                        {
                            interactableObject = hit.collider.transform.GetComponent<InteractableObject>();

                            if (!interactableObject.IsEthereal() && i > 0)
                            {
                                PickupObject();
                            }
                            else if (interactableObject.IsEthereal())
                            {
                                PickupObject();
                            }
                        }

                        if (hit.collider.tag == "Mirror")
                        {
                            relativeMirror = hit.collider.transform;
                        }
                        else
                        {
                            break;
                        }

                        ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                    }
                }
            }
            else
            {
                DropObject();
            }
        }
    }

    private void FixedUpdate()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        mouseScroll = Input.GetAxis("Mouse ScrollWheel");

        if (interactableObject != null)
        {
            MoveObject();
        }
    }

    void PickupObject()
    {
        if (interactableObject.transform.GetComponent<Rigidbody>())
        {
            interactableObjectRB = interactableObject.transform.GetComponent<Rigidbody>();
            interactableObjectRB.useGravity = false;
            interactableObjectRB.drag = 10f;
            interactableObjectRB.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void MoveObject()
    {
        var dir = (transform.position - relativeMirror.transform.position).normalized;
        var forwardBackwardsDir = new Vector3(dir.x, 0, dir.z);
        var leftRightDir = Vector3.Cross(forwardBackwardsDir, Vector3.up);

        Vector3 upDownForce = Vector3.up * mouseY * objectMoveSpeed * 50;
        Vector3 leftRightForce = leftRightDir * mouseX * objectMoveSpeed * 50;

        Vector3 forwardBackwardsForce = Vector3.zero;
        if (mouseScroll > 0)
            forwardBackwardsForce = -forwardBackwardsDir * mouseScrollSense * objectMoveSpeed * 50;
        else if (mouseScroll < 0)
            forwardBackwardsForce = forwardBackwardsDir * mouseScrollSense * objectMoveSpeed * 50;

        interactableObjectRB.AddForce(upDownForce + leftRightForce + forwardBackwardsForce);
    }

    void DropObject()
    {
        interactableObjectRB.useGravity = true;
        interactableObjectRB.drag = 1;
        interactableObjectRB.constraints = RigidbodyConstraints.None;
        interactableObject.transform.parent = null;
        interactableObject = null;
    }



}