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

    [Header("Optional Mirror to Show + Deal with Elevator Problems")]
    public GameObject mirrorToShow;
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
        //Highly Personalized to specificly -z direction. Can be expanded for others if need be. -Nick
         if(zAxis==true && (minZ >= transform.position.z-.1f)){
            if(mirrorToShow.activeSelf==false){
                mirrorToShow.SetActive(true);
            }
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
