using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Grounded,
    AirBorn,
    GrabbingLedge,
    ClimbingLedge
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public PlayerState state;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    // player height is height of capsule in inspector
    public float playerHeight;
    public LayerMask GroundLayer;
    public LayerMask InteractableLayer;
    private int CombinedLayers;
    private RaycastHit groundHit;
    private GameObject interactableObject;

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
    private bool ledgeCheck1;
    private bool ledgeCheck2;
    private RaycastHit ledge;

    //private bool climbingUp;
    private Vector3 climbUpHeight;
    private Vector3 climbUpForward;

    public Transform playerCam;

    private Dictionary<KeyCode, bool> keys = new Dictionary<KeyCode, bool>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        CombinedLayers = GroundLayer | InteractableLayer;
    }

    // Update is called once per frame
    void Update()
    {
        // ground check
        CheckState();
        StandingOnInteractableCheck();

        // check for ledge
        ledgeCheck1 = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + playerHeight * 2f - 0.2f, transform.position.z), transform.forward, out ledge, 1.2f, CombinedLayers);
        ledgeCheck2 = !Physics.Raycast(new Vector3(transform.position.x, transform.position.y + playerHeight * 2f, transform.position.z), transform.forward, 2f, CombinedLayers);

        PlayerInput();
        SpeedControl();

        // drag
        if (state == PlayerState.Grounded)
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if (state != PlayerState.GrabbingLedge && state != PlayerState.ClimbingLedge)
        {
            MovePlayer();
        }
        else if (state == PlayerState.ClimbingLedge)
        {
            ClimbUpFromLedge();
        }
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // jump
        if (GetKey(jumpKey) && readyToJump && state == PlayerState.Grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        // get up from ledge
        else if (GetKeyDown(jumpKey) && state == PlayerState.GrabbingLedge)
        {
            state = PlayerState.ClimbingLedge;
            ClimbUpFromLedge();
        }
    }

    private void CheckState()
    {
        if (state != PlayerState.GrabbingLedge && state != PlayerState.ClimbingLedge)
        {
            bool grounded = Physics.SphereCast(transform.position + new Vector3(0, 0.5f, 0), 0.25f, Vector3.down, out groundHit, 0.5f, CombinedLayers);
            if (grounded)
            {
                state = PlayerState.Grounded;
            }
            else
            {
                state = PlayerState.AirBorn;
            }
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (state == PlayerState.Grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            EnableKey(jumpKey);
        }
        // in air
        else if (state == PlayerState.AirBorn)
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

    private void StandingOnInteractableCheck()
    {
        if (groundHit.transform != null)
        {
            if (groundHit.transform.GetComponent<InteractableObject>())
            {
                interactableObject = groundHit.transform.gameObject;
                interactableObject.GetComponent<InteractableObject>().DisableInteraction();
            }
        }
        else
        {
            if (interactableObject && state != PlayerState.GrabbingLedge)
            {
                interactableObject.GetComponent<InteractableObject>().EnableInteraction();
                interactableObject = null;
            }
        }
    }

    private void GrabLedge()
    {
        climbUpHeight = new Vector3(transform.position.x, transform.position.y + playerHeight * 2f, transform.position.z);
        climbUpForward = new Vector3(transform.position.x + transform.forward.x * forwardDistance, transform.position.y + playerHeight * 2f, transform.position.z + transform.forward.z * forwardDistance);
        state = PlayerState.GrabbingLedge;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        playerCam.GetComponent<PlayerCam>().limitYRotation = transform.rotation.eulerAngles.y;
        
        if (ledge.transform.GetComponent<InteractableObject>())
        {
            interactableObject = ledge.transform.gameObject;
            interactableObject.GetComponent<InteractableObject>().DisableInteraction();
        }
    }

    private void ClimbUpFromLedge()
    {
        if (transform.position.y != climbUpHeight.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, climbUpHeight, climbUpSpeed * Time.deltaTime);
            DisableKey(jumpKey);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, climbUpForward, climbUpSpeed * Time.deltaTime);
            if (transform.position == climbUpForward)
            {
                state = PlayerState.Grounded;
                rb.useGravity = true;
                DisableMovement();
                
                if (interactableObject)
                {
                    interactableObject.GetComponent<InteractableObject>().EnableInteraction();
                    interactableObject = null;
                }
            }
        }
    }

    public void ChangeJumpForce(float value)
    {
        jumpForce = value;
    }


    public void ChangeMovementSpeed(float value)
    {
        moveSpeed = value;
    }

    public void ChangeClimbUpSpeed(float value)
    {
        climbUpSpeed = value;
    }

    private bool GetKeyDown(KeyCode key)
    {
        if (!keys.ContainsKey(key))
        {
            keys.Add(key, true);
        }
        return Input.GetKeyDown(key) && keys[key];
    }

    private bool GetKey(KeyCode key)
    {
        if (!keys.ContainsKey(key))
        {
            keys.Add(key, true);
        }
        return Input.GetKey(key) && keys[key];
    }

    private bool GetKeyUp(KeyCode key)
    {
        if (!keys.ContainsKey(key))
        {
            keys.Add(key, true);
        }
        return Input.GetKeyUp(key) && keys[key];
    }

    private void DisableKey(KeyCode key)
    {
        if (!keys.ContainsKey(key))
        {
            keys.Add(key, false);
        }
        else
        {
            keys[key] = false;
        }
    }

    private void EnableKey(KeyCode key)
    {
        if (!keys.ContainsKey(key))
        {
            keys.Add(key, true);
        }
        else
        {
            keys[key] = true;
        }
    }

    private void DisableMovement()
    {
        DisableKey(KeyCode.W);
        DisableKey(KeyCode.A);
        DisableKey(KeyCode.S);
        DisableKey(KeyCode.D);
    }

}
