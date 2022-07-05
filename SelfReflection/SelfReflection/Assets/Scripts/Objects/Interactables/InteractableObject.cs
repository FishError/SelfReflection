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
        state = ObjectState.Interactable;
    }

    public override void MoveObject(float mouseX, float mouseY, float mouseScroll, Vector3 rayDir, Vector3 playerPosition, Vector3 mirrorPosition)
    {
        MoveRelativeToPlayer(mouseX, mouseY, mouseScroll, rayDir, playerPosition);
    }

    public void MoveRelativeToPlayer(float mouseX, float mouseY, float mouseScroll, Vector3 rayDir, Vector3 playerPosition)
    {
        var forwardBackwardDir = new Vector3(rayDir.x, 0, rayDir.z);
        var leftRightDir = Vector3.Cross(forwardBackwardDir, Vector3.up);
        var playerObjectDistance = (playerPosition - transform.position).magnitude;

        Vector3 upDownVelocity = Vector3.up * mouseY * playerObjectDistance;
        Vector3 leftRightVelocity = leftRightDir * mouseX * playerObjectDistance;
        Vector3 forwardBackwardVelocity = -forwardBackwardDir * mouseScroll;
        Vector3 velocity = upDownVelocity + leftRightVelocity + forwardBackwardVelocity;

        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.ClampMagnitude(velocity, maxVelocity), 0.3f);
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
