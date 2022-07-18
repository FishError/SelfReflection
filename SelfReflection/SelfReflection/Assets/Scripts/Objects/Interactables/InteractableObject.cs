using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// should be attached to all interactable objects
public class InteractableObject : Interactable
{
    protected Collider playerCollider;

    public override void SelectObject(InteractionController controller, Interaction interaction)
    {
        /*
        if (moveObjectController.relativeMirror)
        {
            state = ObjectState.MovingThroughMirror;
        }
        else
        {
            state = ObjectState.Holding;
        }*/

        interactionController = controller;
        switch (interaction)
        {
            case Interaction.PickUp:
            case Interaction.Holding:
            case Interaction.MirrorMove:
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                break;
        }
    }

    public override void UnSelectObject()
    {
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        interactionController = null;
        
        if (state == ObjectState.Holding)
            Physics.IgnoreCollision(playerCollider, GetComponent<Collider>(), false);

        state = ObjectState.Interactable;
    }

    public void PickUp(Transform pickUpParent)
    {
        if (transform.position != pickUpParent.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, pickUpParent.position, 15 * Time.deltaTime);
        }

    }

    public override void MoveObject(float mouseX, float mouseY, float mouseScroll, Vector3 rayDir, Vector3 playerPosition)
    {
        Vector3 velocity = CalculateVelocity(mouseX, mouseY, mouseScroll, rayDir, playerPosition);
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.ClampMagnitude(velocity, maxVelocity), 0.3f);
    }

    public void SwapState(Vector3 mirrorSpawnLocation, Transform pickUpParent)
    {
        if (IsEthereal() && canSwapStates)
        {
            SetToReal();
            transform.position = mirrorSpawnLocation;
            transform.parent = null;
        }
        else if (!IsEthereal() && canSwapStates)
        {
            SetToEthereal();
            transform.position = mirrorSpawnLocation;
            transform.parent = pickUpParent;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == ObjectState.MovingThroughMirror)
            interactionController.DropObject();

        if (collision.gameObject.layer == player.layer && state == ObjectState.Holding)
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
            playerCollider = collision.collider;
            var distance = (transform.position - collision.GetContact(0).point).magnitude;
            interactionController.ScalePickUpParentRange(distance + 1f);
        }
    }
}
