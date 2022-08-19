using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectLimitedMovement : InteractableObject
{
    [Header("Movement Limitations")]
    public float relativeMinX;
    public float relativeMaxX;
    public float relativeMinY;
    public float relativeMaxY;
    public float relativeMinZ;
    public float relativeMaxZ;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float minZ;
    private float maxZ;

    [Header("Reset Settings")]
    public float resetTimer;
    private float timeLeft;
    private Vector3 originalPosition;

    protected override void Start()
    {
        base.Start();
        minX = transform.position.x + relativeMinX;
        maxX = transform.position.x + relativeMaxX;
        minY = transform.position.y + relativeMinY;
        maxY = transform.position.y + relativeMaxY;
        minZ = transform.position.z + relativeMinZ;
        maxZ = transform.position.z + relativeMaxZ;

        originalPosition = transform.position;
    }

    private void Update()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        x = Mathf.Clamp(transform.position.x, minX, maxX);
        y = Mathf.Clamp(transform.position.y, minY, maxY);
        z = Mathf.Clamp(transform.position.z, minZ, maxZ);

        if (OutOfBounds())
        {
            transform.position = new Vector3(x, y, z);
        }

        if (transform.position != originalPosition && interactionState != Interaction.MirrorMove)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0f)
            {
                transform.position = originalPosition;
            }
        }
    }

    public override void UnSelectObject()
    {
        rb.constraints = RigidbodyConstraints.None;
        interactionController = null;

        if (interactionState == Interaction.Holding)
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(playerCollider, c, true);
            }

        interactionState = Interaction.None;
        timeLeft = resetTimer;
    }

    private bool OutOfBounds()
    {
        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.y < minY || transform.position.y > maxY ||
            transform.position.z < minZ || transform.position.z > maxZ)
        {
            return true;
        }

        return false;
    }
}
