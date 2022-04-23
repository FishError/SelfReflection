using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectController : MonoBehaviour
{
    [Header("Pickup Settings")]
    public InteractableObject interactableObject;
    public int interactableLayerIndex;

    [Header("Physics Parameters")]
    public RaycastHit hit;
    private Ray ray;
    public float maxDistance = 150f;
    public int maxReflections;
    private float mouseX, mouseY, mouseScroll;
    public float sensX;
    public float sensY;
    public float mouseScrollSense;
    public float objectMoveSpeed;

    public Transform relativeMirror;

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
                print("hello?");
                ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
                for (int i = 0; i < maxReflections; i++)
                {
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, maxDistance))
                    {
                        if (hit.collider.transform.gameObject.layer == interactableLayerIndex)
                        {
                            interactableObject = hit.collider.transform.GetComponent<InteractableObject>();

                            if (!interactableObject.IsEthereal() && i > 0)
                            {
                                SelectInterableObject();
                            }
                            else if (interactableObject.IsEthereal() && i == 0)
                            {
                                SelectInterableObject();
                            }
                            else
                            {
                                interactableObject = null;
                            }
                        }

                        if (hit.collider.tag == "Mirror")
                        {
                            relativeMirror = hit.collider.transform;
                        }
                        else
                        {
                            if (interactableObject == null)
                                relativeMirror = null;

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

        if (interactableObject != null && relativeMirror != null)
        {
            MoveObjectThroughMirror();
        }
        else if (interactableObject != null && relativeMirror == null)
        {
            MoveObjectNoMirror();
        }
    }

    void SelectInterableObject()
    {
        if (interactableObject.transform.GetComponent<Rigidbody>())
        {
            interactableObject.SelectObject(this);
        }
    }

    void MoveObjectThroughMirror()
    {
        var dir = (transform.position - relativeMirror.transform.position).normalized;
        var forwardBackwardDir = new Vector3(dir.x, 0, dir.z);
        var leftRightDir = Vector3.Cross(forwardBackwardDir, Vector3.up);

        Vector3 upDownForce = Vector3.up * mouseY * objectMoveSpeed * 50;
        Vector3 leftRightForce = leftRightDir * mouseX * objectMoveSpeed * 50;

        Vector3 forwardBackwardsForce = Vector3.zero;
        if (mouseScroll > 0)
            forwardBackwardsForce = -forwardBackwardDir * mouseScrollSense * objectMoveSpeed * 50;
        else if (mouseScroll < 0)
            forwardBackwardsForce = forwardBackwardDir * mouseScrollSense * objectMoveSpeed * 50;

        interactableObject.AddForce(upDownForce + leftRightForce + forwardBackwardsForce);
    }

    // this shouldn't be the same as moving objects through a mirror but I kept the logic similar 
    // because I just wanted a placeholder working implementation for now until we change it later
    void MoveObjectNoMirror()
    {
        var forwardBackwardDir = new Vector3(transform.forward.x, 0, transform.forward.z);
        var leftRightDir = Vector3.Cross(forwardBackwardDir, Vector3.up);

        Vector3 upDownForce = Vector3.up * mouseY * objectMoveSpeed * 50;
        Vector3 leftRightForce = -leftRightDir * mouseX * objectMoveSpeed * 50;

        Vector3 forwardBackwardsForce = Vector3.zero;
        if (mouseScroll > 0)
            forwardBackwardsForce = forwardBackwardDir * mouseScrollSense * objectMoveSpeed * 50;
        else if (mouseScroll < 0)
            forwardBackwardsForce = -forwardBackwardDir * mouseScrollSense * objectMoveSpeed * 50;

        interactableObject.AddForce(upDownForce + leftRightForce + forwardBackwardsForce);
    }

    public void DropObject()
    {
        interactableObject.UnselectObject();
        interactableObject = null;
        relativeMirror = null;
    }
}