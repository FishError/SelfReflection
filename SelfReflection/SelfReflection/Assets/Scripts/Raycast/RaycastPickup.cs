using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPickup : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] private Transform pickupParent;
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;

    [Header("Physics Parameters")]
    public RaycastHit hit;
    private Ray ray;
    public float remainingLength;
    public int reflections;
    private float mouseX, mouseY;
    public float sensX;
    public float sensY;

    [Header("InteractableInfo")]
    public int interactableLayerIndex;
    private PhysicsObject physicsObject;
    public GameObject lookObject;
    public Camera mainCamera;
    private Vector3 raycastPos;
    public float sphereCastRadius = 0.5f;


    [Header("ObjectFollow")]
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 300f;
    [SerializeField] private float maxDistance = 100f;
    private float currentSpeed = 0f;
    private float currentDist = 0f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    Quaternion lookRot;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        if (Input.GetMouseButtonDown(0))
        {
            print("Left Clicked");
            if (currentlyPickedUpObject == null)
            {
                print("Object is null");
                ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
                //raycastPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                for (int i = 0; i < reflections; i++)
                {
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, maxDistance, 1 << interactableLayerIndex))
                    //if (Physics.SphereCast(ray.origin, sphereCastRadius, ray.direction, out hit, maxDistance, 1 << interactableLayerIndex))
                    {
                        //remainingLength -= Vector3.Distance(ray.origin, hit.point);
                        ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                        print("Reflecting Ray");
                        print(hit.collider.tag);
                        if (hit.collider.CompareTag("Cube") && i > 0)
                        {
                            //PickupObject(hit.transform.gameObject);
                            lookObject = hit.collider.transform.gameObject;
                            Pickup();
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
                //ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));

            }
            else
            {
                BreakConnection();
            }
        }

        if(currentlyPickedUpObject != null && currentDist > maxDistance)
        {
            BreakConnection();
        }


        if (Input.GetMouseButtonDown(1))
        {
            print("Right clicked");

        }


        /*
        if (currentlyPickedUpObject != null)
        {
            pickupRB.transform.position = new Vector3(pickupRB.transform.position.x, pickupRB.transform.position.y + mouseY, pickupRB.transform.position.z + mouseX);
        }*/
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
            
            //currentlyPickedUpObject.transform.position = pickupParent.transform.position;
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
        physicsObject.raycastPickup = this;
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
