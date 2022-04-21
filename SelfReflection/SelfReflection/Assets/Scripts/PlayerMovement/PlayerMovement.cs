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
    [Header("Ledge Grabbing")]
    public float climbUpSpeed;
    public float forwardDistance;

    [HideInInspector]
    public bool grabbingLedge;
    private bool ledgeCheck1;
    private bool ledgeCheck2;
    private RaycastHit ledge;

    private bool climbingUp;
    private Vector3 climbUpHeight;
    private Vector3 climbUpForward;

    public Transform playerCam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        grabbingLedge = false;
        climbingUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        // check for ledge
        ledgeCheck1 = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + playerHeight - 0.1f, transform.position.z), transform.forward, out ledge, 1.2f, Ground);
        ledgeCheck2 = !Physics.Raycast(new Vector3(transform.position.x, transform.position.y + playerHeight, transform.position.z), transform.forward, 2f, Ground);

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
        else if (climbingUp)
        {
            ClimbUpFromLedge();
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
            climbingUp = true;
            ClimbUpFromLedge();
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // in air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            if (ledgeCheck1 && ledgeCheck2)
            {
                GrabLedge();
            }
        }
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

    private void GrabLedge()
    {
        climbUpHeight = new Vector3(transform.position.x, transform.position.y + playerHeight + 1, transform.position.z);
        climbUpForward = new Vector3(transform.position.x + transform.forward.x * forwardDistance, transform.position.y + playerHeight + 1, transform.position.z + transform.forward.z * forwardDistance);
        grabbingLedge = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        playerCam.GetComponent<PlayerCam>().limitYRotation = transform.rotation.eulerAngles.y;
        print(playerCam.GetComponent<PlayerCam>().limitYRotation);
    }

    private void ClimbUpFromLedge()
    {
        if (transform.position.y != climbUpHeight.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, climbUpHeight, climbUpSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, climbUpForward, climbUpSpeed * Time.deltaTime);
            if (transform.position == climbUpForward)
            {
                grabbingLedge = false;
                rb.useGravity = true;
                climbingUp = false;
            }
        }
    }

    public void ChangeJumpForce(float value)
    {
        jumpForce = value;
    }
}
