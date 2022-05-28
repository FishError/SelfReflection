using UnityEngine;

public class InteractableMirror : Interactable
{
    [Header("Enable Axis Movement")]
    public bool xAxis;
    public float relativeMinX;
    public float relativeMaxX;

    public bool yAxis;
    public float relativeMinY;
    public float relativeMaxY;

    public bool zAxis;
    public float relativeMinZ;
    public float relativeMaxZ;

    private Vector3 distance;
    private float vX = 0;
    private float vY = 0;
    private float VZ = 0;

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
        maxX = transform.position.x + relativeMaxX;
        minY = transform.position.y + relativeMinY;
        maxY = transform.position.y + relativeMaxY;
        minZ = transform.position.z + relativeMinZ;
        maxZ = transform.position.z + relativeMaxZ;
    }

    private void Update()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        if (transform.position.x < minX)
        {
            x = minX;
        }
        else if (transform.position.x > maxX)
        {
            x = maxX;
        }

        if (transform.position.y < minY)
        {
            y = minY;
        }
        else if (transform.position.y > maxY)
        {
            y = maxY;
        }

        if (transform.position.z < minZ)
        {
            z = minZ;
        }
        else if (transform.position.z > maxZ)
        {
            z = maxZ;
        }

        if (OutOfBounds())
        {
            transform.position = new Vector3(x, y, z);
        }
    }

    private void FixedUpdate()
    {
        if (state == ObjectState.Holding)
        {
            var dir = (moveObjectController.transform.position + distance) - transform.position;
            if (xAxis)
            {
                vX = dir.x;
            }
            if (yAxis)
            {
                vY = dir.y;
            }
            if (zAxis)
            {
                VZ = dir.z;
            }

            rb.velocity = new Vector3(vX, vY, VZ) * 5;
        }
    }

    public override void SelectObject(MoveObjectController controller)
    {
        moveObjectController = controller;
        rb.mass = 0;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        state = ObjectState.Holding;

        distance = transform.position - controller.transform.position;
    }

    public override void UnSelectObject()
    {
        moveObjectController = null;
        rb.mass = mass;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        state = ObjectState.Interactable;
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
