using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // ledge variables
    [HideInInspector]
    public bool grabbingLedge;
    private bool gettingUp;
    private Vector3 GetUpPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        grabbingLedge = false;
        gettingUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        PlayerInput();
        SpeedControl();

        // drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if (!grabbingLedge)
        {
            MovePlayer();
        }
        else if (gettingUp)
        {
            PullUpFromLedge();
        }
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        // get up from ledge
        else if (Input.GetKey(jumpKey) && grabbingLedge)
        {
            gettingUp = true;
            PullUpFromLedge();
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void GrabLedge(Vector3 grabPosition, Vector3 getUpPosition, Vector3 getUpDirection)
    {
        transform.position = new Vector3(transform.position.x, grabPosition.y, transform.position.z);
        GetUpPosition = new Vector3(transform.position.x + getUpDirection.x * 2, getUpPosition.y, transform.position.z + getUpDirection.z * 2);
        print(transform.position);
        print(GetUpPosition);
        grabbingLedge = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    private void PullUpFromLedge()
    {
        transform.position = Vector3.MoveTowards(transform.position, GetUpPosition, 3 * Time.deltaTime);
        if (transform.position == GetUpPosition)
        {
            grabbingLedge = false;
            rb.useGravity = true;
            gettingUp = false;
        }
    }
}
