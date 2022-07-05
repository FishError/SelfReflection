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
    private float vZ = 0;

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

        x = Mathf.Clamp(transform.position.x, minX, maxX);
        y = Mathf.Clamp(transform.position.y, minY, maxY);
        z = Mathf.Clamp(transform.position.z, minZ, maxZ);

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
                vZ = dir.z;
            }

            rb.velocity = new Vector3(vX, vY, vZ) * 5;
        }
    }

    public override void SelectObject(MoveObjectController controller)
    {
        moveObjectController = controller;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        if (controller.relativeMirror == null)
        {
            state = ObjectState.Holding;
        }
        else
        {
            rb.drag = 10;
            state = ObjectState.MovingThroughMirror;
        }

        distance = transform.position - controller.transform.position;
    }

    public override void UnSelectObject()
    {
        moveObjectController = null;
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

    public override void MoveObject(float mouseX, float mouseY, float mouseScroll, Vector3 rayDir, Vector3 playerPosition)
    {
        Vector3 velocity = CalculateVelocity(mouseX, mouseY, mouseScroll, rayDir, playerPosition);

        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.ClampMagnitude(velocity, maxVelocity), 0.3f);
    }
}
