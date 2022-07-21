using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlatformLimitedMovement : InteractablePlatform
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
        maxX = transform.position.x + relativeMaxX;
        minY = transform.position.y + relativeMinY;
        maxY = transform.position.y + relativeMaxY;
        minZ = transform.position.z + relativeMinZ;
        maxZ = transform.position.z + relativeMaxZ;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

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

    protected override void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
        }*/
    }

    protected override void OnCollisionExit(Collision collision)
    {
        /*if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
        }*/
    }
}
