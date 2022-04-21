using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupThroughMirror : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] private Transform pickupParent;
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;

    [Header("Physics Parameters")]
    public RaycastHit hit;
    private Ray ray;
    public int reflections;

    [Header("InteractableInfo")]
    public int interactableLayerIndex;
    private PhysicsObject physicsObject;
    public GameObject lookObject;
    public Camera mainCamera;
    public float sphereCastRadius = 0.5f;
    [SerializeField] private Material ethereal;
    [SerializeField] private Material real;


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
            print("Right Clicked");
            if (currentlyPickedUpObject == null)
            {
                ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
                for (int i = 0; i < reflections; i++)
                {
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, maxDistance, 1 << interactableLayerIndex))
                    {
                        ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                        if(i == 0)
                        {
                            spawnLocation = hit.point;
                        }
                        if (hit.collider.CompareTag("Real") && i > 0)
                        {
                            hit.collider.gameObject.tag = "Ethereal";
                            hit.collider.gameObject.GetComponent<MeshRenderer>().material = ethereal;
                            hit.collider.gameObject.transform.position = spawnLocation;
                            lookObject = hit.collider.transform.gameObject;
                            Pickup();
                        }
                        else if(hit.collider.CompareTag("Ethereal") && i > 0)
                        {
                            hit.collider.gameObject.tag = "Real";
                            hit.collider.gameObject.GetComponent<MeshRenderer>().material = real;
                            hit.collider.gameObject.transform.position = spawnLocation;
                            lookObject = hit.collider.transform.gameObject;
                        }
                        
                        if (hit.collider.tag != "Mirror")
                        {
                            break;
                        }
                    }
                    else
                    {
                        lookObject = null;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
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
            currentDist = Vector3.Distance(pickupParent.position, pickupRB.position);
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            currentSpeed += Time.fixedDeltaTime;
            Vector3 direction = pickupParent.position - pickupRB.position;
            pickupRB.velocity = direction.normalized * currentSpeed;


            //Rotation
            lookRot = Quaternion.LookRotation(pickupParent.transform.position - pickupRB.position);
            lookRot = Quaternion.Slerp(pickupParent.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
            pickupRB.MoveRotation(lookRot);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pickupParent.position, 0.5f);
    }


    public void Pickup()
    {
        physicsObject = lookObject.GetComponent<PhysicsObject>();
        currentlyPickedUpObject = lookObject;
        print(currentlyPickedUpObject.name);
        pickupRB = currentlyPickedUpObject.GetComponent<Rigidbody>();
        pickupRB.constraints = RigidbodyConstraints.FreezeRotation;
        pickupRB.mass = 0;
        physicsObject.pickupThroughMirror = this;
        StartCoroutine(physicsObject.PickUp());
    }

    public void BreakConnection()
    {
        pickupRB.constraints = RigidbodyConstraints.None;
        currentlyPickedUpObject = null;
        physicsObject.pickedUp = false;
        currentDist = 0;
        pickupRB.mass = 1000;
    }


    /*
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
    */
    

}
