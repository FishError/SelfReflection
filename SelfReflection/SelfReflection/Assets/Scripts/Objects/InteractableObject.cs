using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// should be attached to all interactable objects
public class InteractableObject : MonoBehaviour
{

    [SerializeField] private Material ethereal;
    [SerializeField] private Material real;
    [HideInInspector] public Rigidbody rb;
    private bool selectedByPlayer;
    private MoveObjectController moveObjectController;
    private PickupThroughMirrorController pickUpThroughMirrorController;
    private GameObject player;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();

        player = GameObject.Find("Player");
        if (player)
            Physics.IgnoreCollision(player.GetComponentInChildren<Collider>(), GetComponent<Collider>(), true);

        if (IsEthereal())
        {
            transform.GetComponent<MeshRenderer>().material = ethereal;
            Physics.IgnoreCollision(player.GetComponentInChildren<Collider>(), GetComponent<Collider>(), false);
        }
        else
            transform.GetComponent<MeshRenderer>().material = real;
    }

    public bool IsEthereal()
    {
        if (transform.tag == "Ethereal")
            return true;

        return false;
    }

    public void SetToEthereal()
    {
        transform.tag = "Ethereal";
        transform.GetComponent<MeshRenderer>().material = ethereal;

        Physics.IgnoreCollision(player.GetComponentInChildren<Collider>(), GetComponent<Collider>(), false);
    }

    public void SetToReal()
    {
        transform.tag = "Real";
        transform.GetComponent<MeshRenderer>().material = real;

        Physics.IgnoreCollision(player.GetComponentInChildren<Collider>(), GetComponent<Collider>(), true);
    }

    public void SelectObject(MoveObjectController controller)
    {
        rb.useGravity = false;
        rb.drag = 10f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        selectedByPlayer = true;
        moveObjectController = controller;
    }

    public void SelectObject(PickupThroughMirrorController controller)
    {
        rb.mass = 0f;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        selectedByPlayer = true;
        pickUpThroughMirrorController = controller;
    }

    public void UnselectObject()
    {
        rb.useGravity = true;
        rb.mass = 1f;
        rb.drag = 1f;
        rb.constraints = RigidbodyConstraints.None;
        selectedByPlayer = false;
        moveObjectController = null;
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (selectedByPlayer && moveObjectController)
            moveObjectController.DropObject();
    }
}
