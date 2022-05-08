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

    private bool colliding;

    public override void SelectObject(MoveObjectController controller)
    {
        moveObjectController = controller;
        state = ObjectState.MovingThroughMirror;
    }

    public override void UnSelectObject()
    {
        moveObjectController = null;
        state = ObjectState.Interactable;
    }

    public override void AddForce(Vector3 x, Vector3 y, Vector3 z)
    {
        if (!colliding)
        {
            if (xAxis)
                transform.position += x.normalized * speed * Time.deltaTime;
            if (yAxis)
                transform.position += y.normalized * speed * Time.deltaTime;
            if (zAxis)
                transform.position += z.normalized * speed * 20 * Time.deltaTime;
        }
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
