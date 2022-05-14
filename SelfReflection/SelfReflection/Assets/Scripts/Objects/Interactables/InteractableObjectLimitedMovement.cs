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

    protected override void Start()
    {
        base.Start();
        minX = transform.position.x + relativeMinX;
        maxX = transform.position.x + relativeMinX;
        minY = transform.position.y + relativeMinY;
        maxY = transform.position.y + relativeMaxY;
        minZ = transform.position.z + relativeMinZ;
        maxZ = transform.position.z + relativeMaxZ;
    }

    private void Update()
    {
        var x = transform.position.x;

        if (transform.position.x < minX || transform.position.x > maxX)
        {

        }

        if (transform.position.y < minY || transform.position.y > maxY)
        {

        }

        if (transform.position.z < minY || transform.position.z > maxZ)
        {

        }
    }
}
