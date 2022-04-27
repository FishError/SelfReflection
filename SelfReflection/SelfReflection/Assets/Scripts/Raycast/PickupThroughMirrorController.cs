using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupThroughMirrorController : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] private Transform pickupParent;
    public GameObject currentlyPickedUpObject;

    [Header("Physics Parameters")]
    public RaycastHit hit;
    private Ray ray;
    public int maxReflections;

    [Header("InteractableInfo")]
    public int interactableLayerIndex;
    private PhysicsObject physicsObject;
    public InteractableObject interactableObject;
    public Camera mainCamera;
    public float sphereCastRadius = 0.5f;

    [Header("ObjectFollow")]
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 300f;
    [SerializeField] private float maxDistance = 100f;
    private float currentSpeed = 0f;
    private float currentDist = 0f;
    private Vector3 spawnLocation;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    Quaternion lookRot;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //print("Right Clicked");
            if (currentlyPickedUpObject == null)
            {
                ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
                for (int i = 0; i < maxReflections; i++)
                {
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, maxDistance))
                    {
                        if(i == 0)
                        {
                            spawnLocation = hit.point;
                        }
                        
                        if (hit.collider.transform.gameObject.layer == interactableLayerIndex)
                        {
                            interactableObject = hit.collider.transform.gameObject.GetComponent<InteractableObject>();

                            if (i > 0 && interactableObject.IsEthereal())
                            {
                                // set object back to real
                                interactableObject.SetToReal();
                                interactableObject.transform.position = spawnLocation;
                            }
                            else if (i > 0 && !interactableObject.IsEthereal())
                            {
                                // pick up object through mirror
                                interactableObject.SetToEthereal();
                                interactableObject.transform.position = spawnLocation;
                                Pickup();
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
        }
        if (currentlyPickedUpObject != null && Input.GetMouseButtonDown(0))
        {
            BreakConnection();
        }
        
        if(currentlyPickedUpObject != null && currentDist > maxDistance)
        {
            BreakConnection();
        }
    }

    private void FixedUpdate()
    {
        if(currentlyPickedUpObject != null)
        {
            currentDist = Vector3.Distance(pickupParent.position, interactableObject.rb.position);
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            currentSpeed += Time.fixedDeltaTime;
            Vector3 direction = pickupParent.position - interactableObject.rb.position;
            interactableObject.rb.velocity = direction.normalized * currentSpeed;


            //Rotation
            lookRot = Quaternion.LookRotation(pickupParent.transform.position - interactableObject.rb.position);
            lookRot = Quaternion.Slerp(pickupParent.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
            interactableObject.rb.MoveRotation(lookRot);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pickupParent.position, 0.5f);
    }


    public void Pickup()
    {
        physicsObject = interactableObject.GetComponent<PhysicsObject>();
        currentlyPickedUpObject = interactableObject.gameObject;
        //print(currentlyPickedUpObject.name);
        interactableObject.SelectObject(this);
        physicsObject.pickupThroughMirror = this;
        StartCoroutine(physicsObject.PickUp());
    }

    public void BreakConnection()
    {
        currentlyPickedUpObject = null;
        physicsObject.pickedUp = false;
        currentDist = 0;
        interactableObject.UnselectObject();
    }
}
