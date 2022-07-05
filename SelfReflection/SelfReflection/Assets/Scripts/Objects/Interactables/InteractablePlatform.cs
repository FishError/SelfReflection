using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlatform : Interactable
{
    [Header("Enable Axis Movement")]
    public bool xAxis;
    public bool yAxis;
    public bool zAxis;

    [Header("Reset Settings")]
    public float resetTimer;
    protected float timeLeft;
    protected Vector3 originalPosition;

    protected Vector3 playerPos;

    protected override void Start()
    {
        base.Start();
        originalPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (transform.position != originalPosition && state != ObjectState.MovingThroughMirror)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0f)
            {
                transform.position = originalPosition;
            }
        }
    }

    public override void SelectObject(MoveObjectController controller)
    {
        moveObjectController = controller;
        state = ObjectState.MovingThroughMirror;

        if (!yAxis)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public override void UnSelectObject()
    {
        moveObjectController = null;
        state = ObjectState.Interactable;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        timeLeft = resetTimer;
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

        if (!xAxis)
            velocity -= new Vector3(transform.right.x * velocity.x, transform.right.y * velocity.y, transform.right.z * velocity.z);
        if (!yAxis)
            velocity -= new Vector3(transform.up.x * velocity.x, transform.up.y * velocity.y, transform.up.z * velocity.z);
        if (!zAxis)
            velocity -= new Vector3(transform.forward.x * velocity.x, transform.forward.y * velocity.y, transform.forward.z * velocity.z);

        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.ClampMagnitude(velocity, maxVelocity), 0.3f);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
            collision.transform.GetComponent<Rigidbody>().useGravity = false;
            playerPos = collision.transform.localPosition;
        }
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && state == ObjectState.MovingThroughMirror)
        {
            collision.transform.localPosition = playerPos;
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
            collision.transform.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
