using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectController : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Interactable interactable;
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
    public Vector3 lastPlayerPosition;

    private PlayerMovement playerMovement;
    private IKController ik;

    private void Start()
    {
        sensX = transform.GetComponent<PlayerCam>().sensX;
        sensY = transform.GetComponent<PlayerCam>().sensY;
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        ik = GameObject.Find("Player").transform.GetChild(2).GetComponent<IKController>();
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

        if (interactable != null)
        {
            if (interactable.state == ObjectState.MovingThroughMirror)
            {
                MoveObjectThroughMirror();
            }
            else if (interactable.state == ObjectState.Holding)
            {
                MoveObjectNoMirror();
            }
        }
    }

    void LeftClick()
    {
        if (interactable == null)
        {
            ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
            for (int reflections = 0; reflections < maxReflections; reflections++)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, maxReflectionDistance))
                {
                    if (hit.collider.transform.gameObject.layer == interactableLayerIndex)
                    {
                        interactable = hit.rigidbody.transform.GetComponent<Interactable>();

                        if (reflections > 0)
                        {
                            SelectInterableObject(true);
                        }
                        else if (interactable.IsEthereal() && hit.distance < maxGrabDistance && reflections == 0)
                        {
                            if (interactable.state == ObjectState.Interactable)
                            {
                                SelectInterableObject(false);
                                interactable.transform.parent = pickupParent;
                                ik.ikActive = true;
                            }
                            else
                            {
                                interactable = null;
                            }
                        }
                        else
                        {
                            interactable = null;
                        }
                    }

                    if (hit.collider.tag == "Mirror")
                    {
                        relativeMirror = hit.collider.transform;
                    }
                    else
                    {
                        if (interactable == null)
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
            ik.ikActive = false;
        }
    }

    void RightClick()
    {
        if (interactable == null)
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
                        interactable = hit.rigidbody.transform.GetComponent<Interactable>();

                        if (reflections > 0 && interactable.IsEthereal() && interactable.canBecomeEthereal)
                        {
                            // set object back to real
                            interactable.SetToReal();
                            interactable.transform.position = spawnLocation;
                            interactable.transform.parent = null;
                            interactable = null;
                        }
                        else if (reflections > 0 && !interactable.IsEthereal() && interactable.canBecomeEthereal)
                        {
                            // pick up object through mirror
                            interactable.SetToEthereal();
                            interactable.transform.position = spawnLocation;
                            interactable.transform.parent = pickupParent;
                            SelectInterableObject(false);
                        }
                        else
                        {
                            interactable = null;
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
                    interactable = null;
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
        if (interactable.transform.GetComponent<Rigidbody>())
        {
            interactable.SelectObject(this);
            lastPlayerPosition = transform.position;
            playerMovement.ledgeGrabbingDisabled = true;
        }
    }

    void MoveObjectThroughMirror()
    {
        if (interactable is InteractableObject)
        {
            var dir = (lastPlayerPosition - relativeMirror.transform.position).normalized;
            var forwardBackwardDir = new Vector3(dir.x, 0, dir.z);
            var leftRightDir = Vector3.Cross(forwardBackwardDir, Vector3.up);
            var playerObjectDistance = (transform.position - interactable.transform.position).magnitude;

            Vector3 upDownForce = Vector3.up * mouseY * objectMoveSpeed * playerObjectDistance * 5;
            Vector3 leftRightForce = leftRightDir * mouseX * objectMoveSpeed * playerObjectDistance * 5;

            Vector3 forwardBackwardsForce = Vector3.zero;
            if (mouseScroll > 0)
                forwardBackwardsForce = -forwardBackwardDir * mouseScrollSense * objectMoveSpeed * 20;
            else if (mouseScroll < 0)
                forwardBackwardsForce = forwardBackwardDir * mouseScrollSense * objectMoveSpeed * 20;

            interactable.MoveRelativeToPlayer(leftRightForce, upDownForce, forwardBackwardsForce);
        }
        else if (interactable is InteractablePlatform)
        {
            interactable.MoveRelativeToObject(mouseX, mouseY, mouseScroll * mouseScrollSense * 5);
        }
    }

    void MoveObjectNoMirror()
    {
        if (interactable.transform.position != pickupParent.position)
        {
            interactable.transform.position = Vector3.MoveTowards(interactable.transform.position, pickupParent.position, 15 * Time.deltaTime);
        }
    }

    public void DropObject()
    {
        interactable.transform.parent = null;
        interactable.UnSelectObject();
        interactable = null;
        relativeMirror = null;
        playerMovement.ledgeGrabbingDisabled = false;
    }

    public void ScalePickUpParentRange(float distance)
    {
        pickupParent.localPosition = new Vector3(pickupParent.localPosition.x, pickupParent.localPosition.y, distance);
    }
}