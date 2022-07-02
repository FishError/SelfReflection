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

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected Material ethereal;
    [SerializeField] protected Material real;
    [HideInInspector] public Rigidbody rb;
    public float maxVelocity;
    public ObjectState state;
    public bool canBecomeEthereal;
    protected MoveObjectController moveObjectController;
    protected GameObject player;
    protected float mass;
    protected float drag;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        state = ObjectState.Interactable;
        rb = transform.GetComponent<Rigidbody>();
        mass = rb.mass;
        drag = rb.drag;

        player = GameObject.Find("Player");

        if (transform.GetComponent<MeshRenderer>() && ethereal != null && real != null)
        {
            if (IsEthereal())
            {
                transform.GetComponent<MeshRenderer>().material = ethereal;
            }
            else
            {
                transform.GetComponent<MeshRenderer>().material = real;
            }
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
    }

    public void SetToReal()
    {
        transform.tag = "Real";
        transform.GetComponent<MeshRenderer>().material = real;
    }

    public void EnableInteraction()
    {
        state = ObjectState.Interactable;
    }

    public void DisableInteraction()
    {
        state = ObjectState.InteractionDisabled;
    }

    public abstract void SelectObject(MoveObjectController controller);

    public abstract void UnSelectObject();

    public abstract void MoveObject(float mouseX, float mouseY, float mouseScroll, Vector3 rayDir, Vector3 playerPosition, Vector3 mirrorPosition);
}
