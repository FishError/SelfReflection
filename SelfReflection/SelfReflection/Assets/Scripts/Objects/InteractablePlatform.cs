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

    private Vector3 xDir = Vector3.zero;
    private Vector3 yDir = Vector3.zero;
    private Vector3 zDir = Vector3.zero;

    private void Update()
    {
        if (state == ObjectState.MovingThroughMirror)
        {
            transform.position += (xDir + yDir + zDir) * speed * Time.deltaTime;
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
    }

    public override void AddForce(Vector3 x, Vector3 y, Vector3 z)
    {
        if (xAxis)
            xDir = x.normalized;
        if (yAxis)
            yDir = y.normalized;
        if (zAxis)
            zDir = z.normalized * 5f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == player.layer)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == player.layer)
        {
            collision.transform.SetParent(null);
        }
    }
}
