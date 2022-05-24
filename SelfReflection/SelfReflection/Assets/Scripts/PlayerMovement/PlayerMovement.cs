using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    MovementDisabled,
    Grounded,
    AirBorn,
    GrabbingLedge,
    ClimbingLedge
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public bool movementDisabled;
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
    private GameObject interactableGroundObject;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // ledge variables
    [Header("Ledge Grabbing")]
    public bool ledgeGrabbingDisabled;
    public Transform downCast;
    public Transform forwardCast;
    public Transform lowLedgeCast;
    public float climbUpSpeed;
    private Vector3 finalClimbUpPosition;

    private RaycastHit downCastHit;
    private RaycastHit forwardCastHit;
    private RaycastHit overHeadCastHit;

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
        if (!movementDisabled)
        {
            // ground check
            CheckState();
            StandingOnInteractableCheck();

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
    }

    private void FixedUpdate()
    {
        if (!movementDisabled)
        {
            if (state == PlayerState.Grounded || state == PlayerState.AirBorn)
            {
                MovePlayer();
            }
            else if (state == PlayerState.ClimbingLedge)
            {
                ClimbUpFromLedge();
            }
        }
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        switch(state)
        {
            case PlayerState.Grounded:
                if (GetKey(jumpKey) && readyToJump)
                {
                    readyToJump = false;
                    Jump();
                    Invoke(nameof(ResetJump), jumpCooldown);
                }
                else if (CheckLedge() && verticalInput > 0)
                {
                    GrabLedge();
                    state = PlayerState.ClimbingLedge;
                }
                break;

            case PlayerState.AirBorn:
                if (CheckLedge() && verticalInput > 0)
                {
                    GrabLedge();
                    state = PlayerState.ClimbingLedge;
                }
                else if (CheckLedge() && forwardCastHit.collider != null)
                {
                    GrabLedge();
                }
                break;

            case PlayerState.GrabbingLedge:
                if (GetKeyDown(jumpKey))
                {
                    state = PlayerState.ClimbingLedge;
                }
                break;

            case PlayerState.ClimbingLedge:
                break;
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

    private bool CheckLedge()
    {
        if (!ledgeGrabbingDisabled)
        {
            var down = Physics.Raycast(downCast.position, Vector3.down, out downCastHit, 3f, CombinedLayers);
            var forward = Physics.Raycast(forwardCast.position, forwardCast.forward, out forwardCastHit, 2f, CombinedLayers);
            var overHead = Physics.Raycast(lowLedgeCast.position, Vector3.down, out overHeadCastHit, 3f, CombinedLayers);

            if (down && forward)
            {
                return true;
            }
            else if (down && overHead)
            {
                return true;
            }
        }

        return false;
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
                interactableGroundObject = groundHit.transform.gameObject;
                interactableGroundObject.GetComponent<InteractableObject>().DisableInteraction();
            }
        }
        else
        {
            if (interactableGroundObject && state != PlayerState.GrabbingLedge && state != PlayerState.ClimbingLedge)
            {
                interactableGroundObject.GetComponent<InteractableObject>().EnableInteraction();
                interactableGroundObject = null;
            }
        }
    }

    private void GrabLedge()
    {
        finalClimbUpPosition = new Vector3(downCastHit.point.x, downCastHit.point.y + 0.1f, downCastHit.point.z);
        state = PlayerState.GrabbingLedge;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        playerCam.GetComponent<PlayerCam>().limitYRotation = transform.rotation.eulerAngles.y;
        
        if (downCastHit.transform.GetComponent<Interactable>())
        {
            interactableGroundObject = downCastHit.transform.gameObject;
            interactableGroundObject.GetComponent<Interactable>().DisableInteraction();
        }
    }

    private void ClimbUpFromLedge()
    {
        if (transform.position.y < finalClimbUpPosition.y)
        {
            rb.velocity = Vector3.up * climbUpSpeed;
            DisableKey(jumpKey);
        }
        else
        {
            float distance = Vector3.Distance(transform.position, finalClimbUpPosition);
            rb.velocity = (finalClimbUpPosition - transform.position).normalized * climbUpSpeed * distance;
            if (distance < 0.1f)
            {
                rb.velocity = Vector3.zero;
                rb.useGravity = true;
                state = PlayerState.Grounded;
                
                if (interactableGroundObject)
                {
                    interactableGroundObject.GetComponent<Interactable>().EnableInteraction();
                    interactableGroundObject = null;
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

    public void DisableMovement()
    {
        DisableKey(KeyCode.W);
        DisableKey(KeyCode.A);
        DisableKey(KeyCode.S);
        DisableKey(KeyCode.D);
        DisableKey(jumpKey);
    }

    public void EnableMovement()
    {
        EnableKey(KeyCode.W);
        EnableKey(KeyCode.A);
        EnableKey(KeyCode.S);
        EnableKey(KeyCode.D);
        EnableKey(jumpKey);
    }
}
