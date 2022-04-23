using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// should be attached to all interactable objects
public class InteractableObject : MonoBehaviour
{

    [SerializeField] private Material ethereal;
    [SerializeField] private Material real;
    private Rigidbody rb;
    private bool selectedByPlayer;
    private MoveObjectController moveObjectController;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();

        if (IsEthereal())
            transform.GetComponent<MeshRenderer>().material = ethereal;
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
    }

    public void SetToReal()
    {
        transform.tag = "Real";
        transform.GetComponent<MeshRenderer>().material = real;
    }

    public void SelectObject(MoveObjectController controller)
    {
        rb.useGravity = false;
        rb.drag = 10f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        selectedByPlayer = true;
        moveObjectController = controller;
    }

    public void UnselectObject()
    {
        rb.useGravity = true;
        rb.drag = 1;
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
        if (selectedByPlayer)
            moveObjectController.DropObject();
    }
}
