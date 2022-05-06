using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlatform : Interactable
{
    [Header("Enable Axis Movement")]
    public bool xAxis;
    public bool yAxis;
    public bool zAxis;

    public override void SelectObject(MoveObjectController controller)
    {
        moveObjectController = controller;
        state = ObjectState.MovingThroughMirror;
    }

    public override void UnSelectObject()
    {
        state = ObjectState.Interactable;
    }

    public override void AddForce(Vector3 x, Vector3 y, Vector3 z)
    {
        if (xAxis)
            rb.AddForce(x);
        if (yAxis)
            rb.AddForce(y);
        if (zAxis)
            rb.AddForce(z);
    }
}
