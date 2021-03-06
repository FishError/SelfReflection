using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// should be attached to all interactable objects
public class InteractableObject : Interactable
{
    protected Collider playerCollider;

    public override void SelectObject(InteractionController controller, Interaction interaction)
    {
        interactionController = controller;
        switch (interaction)
        {
            case Interaction.PickUp:
            case Interaction.Holding:
                interactionState = Interaction.Holding;
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                break;

            case Interaction.MirrorMove:
                interactionState = Interaction.MirrorMove;
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
        
        if (interactionState == Interaction.Holding)
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(playerCollider, c, true);
            }

        interactionState = Interaction.None;
    }

    public void HoldObject(Transform pickUpParent)
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

    public void SwapState(Vector3 mirrorSpawnLocation)
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
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (interactionState == Interaction.MirrorMove)
            interactionController.DropObject();

        if (collision.gameObject.layer == player.layer && interactionState == Interaction.Holding)
        {
            foreach(Collider c in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(collision.collider, c, true);
            }
            playerCollider = collision.collider;
            var distance = (transform.position - collision.GetContact(0).point).magnitude;
            interactionController.ScalePickUpParentRange(distance + 1f);
        }
    }
}
