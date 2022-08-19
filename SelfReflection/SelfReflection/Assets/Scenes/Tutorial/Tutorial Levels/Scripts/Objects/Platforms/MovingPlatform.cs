using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject platform;
    public Transform pointA;
    public Transform pointB;
    public float platformSpeed;
    public bool moving;

    private Transform targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        targetPoint = pointA;
    }

    private void Update()
    {
        if (Vector3.Distance(platform.transform.position, targetPoint.position) < 0.1f)
        {
            if (targetPoint == pointA)
            {
                targetPoint = pointB;
            }
            else
            {
                targetPoint = pointA;
            }
        }

        if (moving)
        {
            var dir = (targetPoint.position - platform.transform.position).normalized;
            platform.transform.position += dir * platformSpeed * Time.deltaTime;
        }
    }

    public void StopPlatform()
    {
        moving = false;
    }

    public void StartPlatform()
    {
        moving = true;
    }
}
