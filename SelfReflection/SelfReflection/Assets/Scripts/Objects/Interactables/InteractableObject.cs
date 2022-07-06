using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// should be attached to all interactable objects
public class InteractableObject : Interactable
{
    protected Collider playerCollider;

    public override void SelectObject(MoveObjectController controller)
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        moveObjectController = controller;

        if (moveObjectController.relativeMirror)
        {
            state = ObjectState.MovingThroughMirror;
        }
        else
        {
            state = ObjectState.Holding;
        }
    }

    public override void UnSelectObject()
    {
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        moveObjectController = null;
        
        if (state == ObjectState.Holding)
            Physics.IgnoreCollision(playerCollider, GetComponent<Collider>(), false);

        state = ObjectState.Interactable;
    }

    public override void MoveObject(float mouseX, float mouseY, float mouseScroll, Vector3 rayDir, Vector3 playerPosition)
    {
        Vector3 velocity = CalculateVelocity(mouseX, mouseY, mouseScroll, rayDir, playerPosition);
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.ClampMagnitude(velocity, maxVelocity), 0.3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == ObjectState.MovingThroughMirror)
            moveObjectController.DropObject();

        if (collision.gameObject.layer == player.layer && state == ObjectState.Holding)
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
            playerCollider = collision.collider;
            var distance = (transform.position - collision.GetContact(0).point).magnitude;
            moveObjectController.ScalePickUpParentRange(distance + 1f);
        }
    }
}
