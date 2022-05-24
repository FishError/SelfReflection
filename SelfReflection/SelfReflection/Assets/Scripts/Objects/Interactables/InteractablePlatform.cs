using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlatform : Interactable
{
    [Header("Enable Axis Movement")]
    public bool xAxis;
    public bool yAxis;
    public bool zAxis;

    public float speed;

    protected Vector3 xDir = Vector3.zero;
    protected Vector3 yDir = Vector3.zero;
    protected Vector3 zDir = Vector3.zero;

    [Header("Reset Settings")]
    public float resetTimer;
    protected float timeLeft;
    protected Vector3 originalPosition;

    protected override void Start()
    {
        base.Start();
        originalPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (state == ObjectState.MovingThroughMirror)
        {
            transform.position += (xDir + yDir + zDir) * speed * Time.deltaTime;
        }

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

    public override void MoveRelativeToPlayer(float mouseX, float mouseY, float mouseScroll, Vector3 playerPosition, Vector3 mirrorPosition)
    {
        var dir = (playerPosition - mirrorPosition).normalized;
        var forwardBackwardDir = new Vector3(dir.x, 0, dir.z);
        var leftRightDir = Vector3.Cross(forwardBackwardDir, Vector3.up);

        if (xAxis)
            xDir = leftRightDir;
        if (yAxis)
            yDir = Vector3.up;
        if (zAxis)
            zDir = forwardBackwardDir;
    }

    public override void MoveRelativeToObject(float mouseX, float mouseY, float mouseScroll)
    {
        if (xAxis)
            xDir = transform.right * mouseX;
        if (yAxis)
            yDir = transform.up * mouseY;
        if (zAxis)
            zDir = transform.forward * mouseScroll * 5f;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
            collision.transform.GetComponent<Rigidbody>().useGravity = false;
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
