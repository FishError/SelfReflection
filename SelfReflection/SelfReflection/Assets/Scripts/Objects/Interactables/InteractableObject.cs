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

    public override void MoveObject(float mouseX, float mouseY, float mouseScroll, Vector3 rayDir, Vector3 playerPosition, Vector3 mirrorPosition)
    {
        MoveRelativeToPlayer(mouseX, mouseY, mouseScroll, rayDir, playerPosition);
    }

    public void MoveRelativeToPlayer(float mouseX, float mouseY, float mouseScroll, Vector3 cameraDir, Vector3 playerPosition)
    {
        var forwardBackwardDir = new Vector3(cameraDir.x, 0, cameraDir.z);
        var leftRightDir = Vector3.Cross(forwardBackwardDir, Vector3.up);
        var playerObjectDistance = (playerPosition - transform.position).magnitude;

        Vector3 upDownForce = Vector3.up * mouseY * playerObjectDistance * 5;
        Vector3 leftRightForce = leftRightDir * mouseX * playerObjectDistance * 5;
        Vector3 forwardBackwardsForce = -forwardBackwardDir * mouseScroll * 20;

        rb.AddForce(leftRightForce + upDownForce + forwardBackwardsForce);
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
