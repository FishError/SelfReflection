using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlatform : Interactable
{
    [Header("Enable Axis Movement")]
    public bool xAxis;
    public bool yAxis;
    public bool zAxis;

    [Header("Reset Settings")]
    public float resetTimer;
    protected float timeLeft;
    protected Vector3 originalPosition;

    protected Vector3 playerPos;

    [Header("Reset Feedback Settings")]
    public float shakeSpeed = 5;
    public float shakeIntensity = 5;
    public float speed = 7;
    private bool isShaking, isStaying;
    private Collision getCollision;

    protected override void Start()
    {
        base.Start();
        originalPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (transform.position != originalPosition && interactionState != Interaction.MirrorMove)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < resetTimer / 2f && timeLeft > 0)
            {
                float step = shakeSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + Random.insideUnitSphere, step);
                isShaking = true;
            }

            if(timeLeft < 0f)
            {
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
                isShaking = false;
            }
            if(getCollision != null)
            {
                CheckCollision();
            }
            
        }

        
    }

    public override void SelectObject(InteractionController controller, Interaction interaction)
    {
        interactionController = controller;
        switch (interaction)
        {
            case Interaction.MirrorMove:
                interactionState = Interaction.MirrorMove;
                if (!yAxis)
                    rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                else
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                break;
            case Interaction.Rotate:
                interactionState = Interaction.Rotate;
                rb.constraints = RigidbodyConstraints.FreezePosition;
                break;
        }
    }

    public override void UnSelectObject()
    {
        interactionController = null;
        interactionState = Interaction.None;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        timeLeft = resetTimer;
    }

    public override void MoveObject(float mouseX, float mouseY, float mouseScroll, Vector3 rayDir, Vector3 playerPosition)
    {
        Vector3 velocity = CalculateVelocity(mouseX, mouseY, mouseScroll, rayDir, playerPosition);
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.ClampMagnitude(AdjustVelocity(velocity), maxVelocity), 0.3f);
    }

    public override void Rotate(float mouseX, float mouseY, Vector3 rayDir)
    {
        transform.RotateAround(transform.position, Vector3.up, mouseX);
        Vector3 axis = Vector3.Cross(new Vector3(rayDir.x, 0, rayDir.z), Vector3.up);
        transform.RotateAround(transform.position, axis, mouseY);
    }

    private Vector3 AdjustVelocity(Vector3 velocity)
    {
        Vector3 v = Vector3.zero;
        if (xAxis)
            v += Vector3.Dot(velocity, transform.right) * transform.right;
        if (yAxis)
            v += Vector3.Dot(velocity, transform.up) * transform.up;
        if (zAxis)
            v += Vector3.Dot(velocity, transform.forward) * transform.forward;

        return v;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        getCollision = collision;
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
            collision.transform.GetComponent<Rigidbody>().useGravity = false;
            playerPos = collision.transform.localPosition;
            isStaying = true;
        }
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && interactionState == Interaction.MirrorMove)
        {
            collision.transform.localPosition = playerPos;
            isStaying = true;
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        getCollision = collision;
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
            collision.transform.GetComponent<Rigidbody>().useGravity = true;
            isStaying = false;
        }
    }

    public void CheckCollision()
    {
        if (getCollision.gameObject.tag == "Player" && isShaking)
        {
            getCollision.transform.SetParent(null);
            getCollision.transform.GetComponent<Rigidbody>().useGravity = true;
        }
        else if (getCollision.gameObject.tag == "Player" && !isShaking && isStaying)
        {
            getCollision.transform.SetParent(transform);
            getCollision.transform.GetComponent<Rigidbody>().useGravity = false;
            playerPos = getCollision.transform.localPosition;
        }
    }

    public override void Resize(float mouseScroll)
    {
        throw new System.NotImplementedException();
    }
}
