using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected Material ethereal;
    [SerializeField] protected Material real;
    [HideInInspector] public Rigidbody rb;
    public Interaction interactionState;
    public bool isInteractable;
    public bool canSwapStates;
    public bool canResize;
    public float maxVelocity;
    public float maxScale;
    public float minScale;
    protected InteractionController interactionController;
    protected GameObject player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        interactionState = Interaction.None;
        rb = transform.GetComponent<Rigidbody>();

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
        isInteractable = true;
    }

    public void DisableInteraction()
    {
        isInteractable = false;
    }

    public abstract void SelectObject(InteractionController controller, Interaction interaction);

    public abstract void UnSelectObject();

    public abstract void MoveObject(float mouseX, float mouseY, float mouseScroll, Vector3 rayDir, Vector3 playerPosition);

    public abstract void Resize(float mouseScroll);

    public Vector3 CalculateVelocity(float mouseX, float mouseY, float mouseScroll, Vector3 rayDir, Vector3 playerPosition)
    {
        var forwardBackwardDir = new Vector3(rayDir.x, 0, rayDir.z);
        var leftRightDir = Vector3.Cross(forwardBackwardDir, Vector3.up);
        var playerObjectDistance = (playerPosition - transform.position).magnitude;

        Vector3 upDownVelocity = Vector3.up * mouseY * playerObjectDistance;
        Vector3 leftRightVelocity = leftRightDir * mouseX * playerObjectDistance;
        Vector3 forwardBackwardVelocity = -forwardBackwardDir * mouseScroll;

        return upDownVelocity + leftRightVelocity + forwardBackwardVelocity;
    }
}
