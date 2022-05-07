using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlatform : Interactable
{
    [Header("Enable Axis Movement")]
    public bool xAxis;
    public bool yAxis;
    public bool zAxis;

    private Vector3 currentPlatformPosition;
    private Vector3 playerLocalPosition;

    protected override void Start()
    {
        base.Start();
        currentPlatformPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (state != ObjectState.MovingThroughMirror)
        {
            transform.position = currentPlatformPosition;
            rb.velocity = Vector3.zero;
        }
        else
        {
            currentPlatformPosition = transform.position;
            if (player.transform.parent == transform)
                player.transform.localPosition = playerLocalPosition;
        }
    }

    public override void SelectObject(MoveObjectController controller)
    {
        moveObjectController = controller;
        state = ObjectState.MovingThroughMirror;
        playerLocalPosition = player.transform.localPosition;
        player.GetComponent<PlayerMovement>().DisableMovement();
    }

    public override void UnSelectObject()
    {
        moveObjectController = null;
        state = ObjectState.Interactable;
        player.GetComponent<PlayerMovement>().DisableMovement();
    }

    public override void AddForce(Vector3 x, Vector3 y, Vector3 z)
    {
        if (rb.velocity.magnitude < maxVelocity)
        {
            if (xAxis)
                rb.AddForce(x * mass);
            if (yAxis)
                rb.AddForce(y * mass);
            if (zAxis)
                rb.AddForce(z * mass);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == player.layer)
        {
            player.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == player.layer)
        {
            player.transform.parent = null;
        }
    }
}
