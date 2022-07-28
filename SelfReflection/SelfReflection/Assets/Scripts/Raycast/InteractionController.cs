using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Interaction
{
    None,
    PickUp,
    Holding,
    MirrorMove,
    SwapState,
    Resize,
    Rotate
}

public class InteractionController : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Interactable interactable;
    public LayerMask interactableLayer;
    public LayerMask physicalLayers;
    [SerializeField] private Transform pickupParent;

    [Header("Reflection Parameters")]
    public RaycastHit hit;
    private Ray ray;
    public float maxReflectionDistance = 150f;
    public float maxGrabDistance = 5f;
    public int maxReflections;

    [Header("Left Click Parameters")]
    private bool leftClicked;
    private Interaction currentLeftClickInteraction;
    private float mouseX, mouseY, mouseScroll;
    private GameObject gameObjectCopy;
    public float sensX;
    public float sensY;
    public float mouseScrollSense;
    public float objectMoveSpeed;

    [Header("Right Click Parameters")]
    private bool rightClicked;
    private Interaction currentRightClickInteraction;
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

        currentRightClickInteraction = Interaction.SwapState;
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
            if (leftClicked && !rightClicked)
            {
                InteractWithObject(currentLeftClickInteraction);
            }
            else if (rightClicked && !leftClicked)
            {
                InteractWithObject(currentRightClickInteraction);
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
                if (Physics.Raycast(ray.origin, ray.direction, out hit, maxReflectionDistance, physicalLayers))
                {
                    if (1 << hit.collider.transform.gameObject.layer == interactableLayer.value)
                    {
                        interactable = hit.transform.GetComponent<Interactable>();

                        if (reflections > 0 && !interactable.IsEthereal())
                        {
                            leftClicked = true;
                            SelectInterableObject(Interaction.MirrorMove);
                            currentLeftClickInteraction = Interaction.MirrorMove;
                        }
                        else if (interactable.IsEthereal() && hit.distance < maxGrabDistance && reflections == 0)
                        {
                            if (interactable.isInteractable)
                            {
                                leftClicked = true;
                                currentLeftClickInteraction = Interaction.PickUp;
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
        }
    }

    void RightClick()
    {
        if (interactable == null)
        {
            ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
            for (int reflections = 0; reflections < maxReflections; reflections++)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, maxReflectionDistance, physicalLayers))
                {
                    if (reflections == 0)
                        spawnLocation = hit.point;

                    if (1 << hit.collider.transform.gameObject.layer == interactableLayer.value && reflections > 0)
                    {
                        interactable = hit.rigidbody.transform.GetComponent<Interactable>();
                        if (interactable.isInteractable)
                            rightClicked = true;
                        else
                            interactable = null;
                    }

                    if (hit.collider.tag != "Mirror")
                        break;

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

    void SelectInterableObject(Interaction interaction)
    {
        if (interactable.transform.GetComponent<Rigidbody>())
        {
            interactable.SelectObject(this, interaction);
            lastPlayerPosition = transform.position;
            playerMovement.ledgeGrabbingDisabled = true;
            ik.ikActive = true;
        }
    }

    void InteractWithObject(Interaction interaction)
    {
        switch(interaction)
        {
            case Interaction.PickUp:
                if (interactable is InteractableObject)
                {
                    // creates compound collider so objects don't go through other objects
                    gameObjectCopy = Instantiate(interactable.transform.gameObject);
                    foreach(MeshRenderer mr in gameObjectCopy.GetComponentsInChildren<MeshRenderer>())
                    {
                        mr.enabled = false;
                    }
                    Destroy(gameObjectCopy.GetComponent<Rigidbody>());
                    gameObjectCopy.transform.parent = pickupParent;
                    gameObjectCopy.transform.localPosition = Vector3.zero;

                    SelectInterableObject(interaction);
                    interactable.transform.parent = pickupParent;
                    currentLeftClickInteraction = Interaction.Holding;
                }
                break;

            case Interaction.Holding:
                if (interactable is InteractableObject)
                {
                    InteractableObject interactableObject = (InteractableObject)interactable;
                    interactableObject.HoldObject(pickupParent);
                }
                break;

            case Interaction.MirrorMove:
                var x = mouseX * objectMoveSpeed;
                var y = mouseY * objectMoveSpeed;
                var z = mouseScroll * mouseScrollSense * objectMoveSpeed * 20;
                interactable.MoveObject(x, y, z, ray.direction, lastPlayerPosition);
                break;

            case Interaction.SwapState:
                if (interactable is InteractableObject && interactable.canSwapStates)
                {
                    InteractableObject interactableObject = (InteractableObject)interactable;
                    interactableObject.SwapState(spawnLocation);

                    if (interactableObject.IsEthereal())
                    {
                        currentLeftClickInteraction = Interaction.PickUp;
                        rightClicked = false;
                        leftClicked = true;
                    }
                    else
                    {
                        DropObject();
                    }
                }
                else
                {
                    rightClicked = false;
                    interactable = null;
                }
                break;

            case Interaction.Resize:
                break;

            case Interaction.Rotate:
                break;

            default:
                print("Something went wrong");
                break;
        }
    }

    public void DropObject()
    {
        Destroy(gameObjectCopy);
        interactable.transform.parent = null;
        interactable.UnSelectObject();
        interactable = null;
        relativeMirror = null;
        playerMovement.ledgeGrabbingDisabled = false;
        ik.ikActive = false;
        rightClicked = false;
        leftClicked = false;
    }

    public void ScalePickUpParentRange(float distance)
    {
        pickupParent.localPosition = new Vector3(pickupParent.localPosition.x, pickupParent.localPosition.y, distance);
    }
}