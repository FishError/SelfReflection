using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// should be attached to all interactable objects
public class InteractableObject : Interactable
{
    public override void SelectObject(MoveObjectController controller)
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        moveObjectController = controller;

        if (moveObjectController.relativeMirror)
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

    public override void UnSelectObject()
    {
        rb.useGravity = true;
        rb.mass = mass;
        rb.drag = drag;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        moveObjectController = null;
        state = ObjectState.Interactable;
    }

    public override void MoveRelativeToPlayer(Vector3 x, Vector3 y, Vector3 z)
    {
        rb.AddForce(x + y + z);
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
