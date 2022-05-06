using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectController : MonoBehaviour
{
    [Header("Pickup Settings")]
    private InteractableObject interactableObject;
    public int interactableLayerIndex;
    [SerializeField] private Transform pickupParent;

    [Header("Reflection Parameters")]
    public RaycastHit hit;
    private Ray ray;
    public float maxReflectionDistance = 150f;
    public float maxGrabDistance = 5f;
    public int maxReflections;

    [Header("Left Click Parameters")]
    private float mouseX, mouseY, mouseScroll;
    public float sensX;
    public float sensY;
    public float mouseScrollSense;
    public float objectMoveSpeed;

    [Header("Right Click Parameters")]
    private Vector3 spawnLocation;

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
            LeftClick();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RightClick();
        }
    }

    private void FixedUpdate()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        mouseScroll = Input.GetAxis("Mouse ScrollWheel");

        if (interactableObject != null)
        {
            if (interactableObject.state == ObjectState.MovingThroughMirror)
            {
                MoveObjectThroughMirror();
            }
            else if (interactableObject.state == ObjectState.Holding)
            {
                MoveObjectNoMirror();
            }
        }
    }

    void LeftClick()
    {
        if (interactableObject == null)
        {
            ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
            for (int reflections = 0; reflections < maxReflections; reflections++)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, maxReflectionDistance))
                {
                    if (hit.collider.transform.gameObject.layer == interactableLayerIndex)
                    {
                        interactableObject = hit.collider.transform.GetComponent<InteractableObject>();

                        if (reflections > 0)
                        {
                            SelectInterableObject(true);
                        }
                        else if (interactableObject.IsEthereal() && hit.distance < maxGrabDistance && reflections == 0)
                        {
                            if (interactableObject.state == ObjectState.Interactable)
                            {
                                SelectInterableObject(false);
                                interactableObject.transform.parent = pickupParent;
                            }
                            else
                            {
                                interactableObject = null;
                            }
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

    void RightClick()
    {
        if (interactableObject == null)
        {
            ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
            for (int reflections = 0; reflections < maxReflections; reflections++)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, maxReflectionDistance))
                {
                    if (reflections == 0)
                    {
                        spawnLocation = hit.point;
                    }

                    if (hit.collider.transform.gameObject.layer == interactableLayerIndex)
                    {
                        interactableObject = hit.collider.transform.gameObject.GetComponent<InteractableObject>();

                        if (reflections > 0 && interactableObject.IsEthereal())
                        {
                            // set object back to real
                            interactableObject.SetToReal();
                            interactableObject.transform.position = spawnLocation;
                            interactableObject.transform.parent = null;
                            interactableObject = null;
                        }
                        else if (reflections > 0 && !interactableObject.IsEthereal())
                        {
                            // pick up object through mirror
                            interactableObject.SetToEthereal();
                            interactableObject.transform.position = spawnLocation;
                            interactableObject.transform.parent = pickupParent;
                            SelectInterableObject(false);
                        }
                        else
                        {
                            interactableObject = null;
                        }
                    }

                    if (hit.collider.tag != "Mirror")
                    {
                        break;
                    }

                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                }
                else
                {
                    interactableObject = null;
                }
            }
        }
        else
        {
            DropObject();
        }
    }

    void SelectInterableObject(bool mirrorSelect)
    {
        if (interactableObject.transform.GetComponent<Rigidbody>())
        {
            if (interactableObject.transform.GetComponent<InteractablePlatform>())
            {

            }
            else
            {
                interactableObject.SelectObject(this);
            }
        }
    }

    void MoveObjectThroughMirror()
    {
        var dir = (transform.position - relativeMirror.transform.position).normalized;
        var forwardBackwardDir = new Vector3(dir.x, 0, dir.z);
        var leftRightDir = Vector3.Cross(forwardBackwardDir, Vector3.up);
        var playerObjectDistance = (transform.position - interactableObject.transform.position).magnitude;

        Vector3 upDownForce = Vector3.up * mouseY * objectMoveSpeed * playerObjectDistance * 5;
        Vector3 leftRightForce = leftRightDir * mouseX * objectMoveSpeed * playerObjectDistance * 5;

        Vector3 forwardBackwardsForce = Vector3.zero;
        if (mouseScroll > 0)
            forwardBackwardsForce = -forwardBackwardDir * mouseScrollSense * objectMoveSpeed * 20;
        else if (mouseScroll < 0)
            forwardBackwardsForce = forwardBackwardDir * mouseScrollSense * objectMoveSpeed * 20;

        interactableObject.AddForce(upDownForce, leftRightForce, forwardBackwardsForce);
    }

    void MoveObjectNoMirror()
    {
        if (interactableObject.transform.position != pickupParent.position)
        {
            interactableObject.transform.position = Vector3.MoveTowards(interactableObject.transform.position, pickupParent.position, 15 * Time.deltaTime);
        }
    }

    public void DropObject()
    {
        interactableObject.transform.parent = null;
        interactableObject.UnSelectObject();
        interactableObject = null;
        relativeMirror = null;
    }

    public void ScalePickUpParentRange(float distance)
    {
        pickupParent.localPosition = new Vector3(pickupParent.localPosition.x, pickupParent.localPosition.y, distance);
    }
}