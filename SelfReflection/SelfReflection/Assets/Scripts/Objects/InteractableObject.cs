using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectState
{
    Interactable,
    MovingThroughMirror,
    Holding,
    InteractionDisabled
}

// should be attached to all interactable objects
public class InteractableObject : MonoBehaviour
{
    [SerializeField] private Material ethereal;
    [SerializeField] private Material real;
    [HideInInspector] public Rigidbody rb;
    public float mass;
    public float drag;
    public ObjectState state;
    private MoveObjectController moveObjectController;
    private GameObject player;

    private void Start()
    {
        state = ObjectState.Interactable;
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
        {
            transform.GetComponent<MeshRenderer>().material = real;
            Physics.IgnoreCollision(player.GetComponentInChildren<Collider>(), GetComponent<Collider>(), false);
        }
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

        Physics.IgnoreCollision(player.GetComponentInChildren<Collider>(), GetComponent<Collider>(), false);
    }

    public void SelectObject(MoveObjectController controller, bool mirrorSelect)
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        moveObjectController = controller;

        if (mirrorSelect)
        {
            rb.mass = 1f;
            rb.drag = 10f;
            state = ObjectState.MovingThroughMirror;
        }
        else
        {
            rb.mass = 0f;
            rb.drag = 1f;
            state = ObjectState.Holding;
        }
    }

    public void UnselectObject()
    {
        rb.useGravity = true;
        rb.mass = mass;
        rb.drag = drag;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        moveObjectController = null;
        state = ObjectState.Interactable;
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    public void EnableInteraction()
    {
        state = ObjectState.Interactable;
    }

    public void DisableInteraction()
    {
        state = ObjectState.InteractionDisabled;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == ObjectState.MovingThroughMirror)
            moveObjectController.DropObject();

        if (collision.gameObject.layer == player.layer && state == ObjectState.Holding)
        {
            var distance = (transform.position - collision.GetContact(0).point).magnitude;
            moveObjectController.ScalePickUpParentRange(distance + 1f);
        }
    }
}
