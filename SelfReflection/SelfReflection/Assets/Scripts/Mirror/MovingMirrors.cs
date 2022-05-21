using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    x,
    y,
    z
}

public class MovingMirrors : MonoBehaviour
{
    public GameObject mirror;

    [Header("Positional Movement")]
    public bool isMoving;
    public Transform pointA;
    public Transform pointB;
    private Transform targetPoint;
    public float moveSpeed;

    [Header("Rotational Movement")]
    public bool isRotating;
    public Axis axisOfRoatation;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        targetPoint = pointA;
    }

    // Update is called once per frame
    void Update()
    {
        MoveMirror();
        RotateMirror();
    }

    private void MoveMirror()
    {
        if (Vector3.Distance(mirror.transform.position, targetPoint.position) < 0.1f)
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

        if (isMoving)
        {
            var dir = (targetPoint.position - mirror.transform.position).normalized;
            mirror.transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    private void RotateMirror()
    {
        if (isRotating)
        {
            switch(axisOfRoatation)
            {
                case Axis.x:
                    mirror.transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f, Space.Self);
                    break;
                case Axis.y:
                    mirror.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
                    break;
                case Axis.z:
                    mirror.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);
                    break;
            }
        }
    }
}
